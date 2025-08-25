using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Interfaces.UnitOfWork;
using CustomDSATrainer.Domain.Validators;
using CustomDSATrainer.Persistence.UnitOfWork;
using FluentValidation.Results;
using System.Runtime.InteropServices.Marshalling;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality for everything that has to do the the <see cref="Problem"/>:
    /// <list type="bullet">
    /// <item>Submitting a problem</item>
    /// <item>Loading a problem</item>
    /// <item>Generating an AI Review for a problem</item>
    /// </list>
    /// </summary>
    public class ProblemService : IProblemService
    {
        private readonly ISubmissionService _submissionService;
        private readonly IPythonAIService _pythonAIService;
        private ICurrentActiveProblemService _currentActiveProblemService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<ProblemService> _logger;

        public ProblemService(
            ISubmissionService submissionService, ICurrentActiveProblemService currentActiveProblemService, IPythonAIService pythonAIService, 
            IUnitOfWork unitOfWork, ILogger<ProblemService> logger)
        {
            _submissionService = submissionService                      ?? throw new ArgumentNullException(nameof(submissionService), "Submission service cannot be null.");
            _currentActiveProblemService = currentActiveProblemService  ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null.");
            _pythonAIService = pythonAIService                          ?? throw new ArgumentNullException(nameof(pythonAIService), "PythonAIService cannot be null.");
            _unitOfWork = unitOfWork                                    ?? throw new ArgumentNullException(nameof(unitOfWork), "UnitOfWork cannot be null.");
            _logger = logger                                            ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }
        
        /// <summary>
        /// Submits a user-generated executable to be tested for the selected problem.
        /// The executable will run through all the test cases. It will have a custom I/O stream from where it will be given the input.
        /// The output will then be caught and compared to the <see cref="TestCase"/>'s expected output.
        /// It will also be continuously checked for time and memory limits.
        /// The problem will then be marked accordingly based on the result of the test case.
        /// </summary>
        /// <param name="problem">The problem that will be tested.</param>
        /// <param name="pathToExe">The path to the user executable</param>
        /// <exception cref="ArgumentNullException">The problem has not been loaded correctly (is null).</exception>
        /// <exception cref="Exception">The validator has deemed that the <see cref="Submission"/> hasn't been initialized correctly.</exception>
        public void SubmitProblem(Problem problem, string pathToExe)
        {
            if (problem == null) { throw new ArgumentNullException(nameof(problem), "Problem cannot be null."); }
            _logger.LogInformation("Submitting problem with ID: {ProblemId}", problem.Id);

            Submission submission = new Submission { ProblemId = problem.Id, Id = 0, PathToExecutable = pathToExe };

            SubmissionValidator validator = new SubmissionValidator();
            ValidationResult validationResult = validator.Validate(submission);

            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.Errors.ToArray().ToString());
            }

            _submissionService.RunSumbission(submission, problem.Inputs, problem.Outputs);
            _submissionService.SaveToDatabase(submission);
            
            _logger.LogInformation("Problem {ProblemId} was submitted susccessfully.", problem.Id);
        }
        
        /// <summary>
        /// Gets a <see cref="Problem"/> from the database based on the given id and loads it.
        /// If it is loaded successfully the user will be able to submit an executable for this problem.
        /// </summary>
        /// <param name="id">The ID of the problem (<see cref="Problem.Id"/>) that needs to be loaded.</param>
        /// <returns>Whether the <see cref="Problem"/> has been loaded successfully.</returns>
        public async Task<bool> LoadProblem(int id)
        {
            _logger.LogInformation("Trying to load problem {ProblemId}", id);
            Problem? currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem != null && currentProblem.Status == ProblemStatus.Solving)
            {
                currentProblem.Status = ProblemStatus.WasSolving;
                await SaveToDatabase(currentProblem);
            }

            currentProblem = await _unitOfWork.ProblemRepository.GetFromId(id);
            _currentActiveProblemService.CurrentProblem = currentProblem;

            if (currentProblem != null)
            {
                if (currentProblem.Status == ProblemStatus.Unsolved)
                {
                    currentProblem.Status = ProblemStatus.Solving;
                    await SaveToDatabase(currentProblem);
                }

                string[] inputs = currentProblem.Inputs.Split('!');
                string[] outputs = currentProblem.Outputs.Split('!');

                _logger.LogInformation("Problem {ProblemId} was loaded successfully.", id);

                return true;
            }

            _logger.LogInformation("There is no problem with ID {ProblemId}", id);

            return false;
        }

        /// <summary>
        /// Generates an <see cref="AIReview"/> for a given problem. 
        /// It will analyze the user source code and it will give an appropriate review.
        /// </summary>
        /// <param name="problem">The selected <see cref="Problem"/>.</param>
        /// <param name="pathToSource">The source code that should be reviewed.</param>
        /// <returns>The generated review.</returns>
        public async Task<string?> AiReview(Problem problem, string pathToSource)
        {
            _logger.LogInformation("Creating an AI review for problem {ProblemId}. Path to source: {PathToSource}", problem.Id, pathToSource);
            if (!File.Exists(pathToSource))
            {
                _logger.LogError("'{PathToSource}' doesn't exit.", pathToSource);
                return null;
            }

            AIReview currentReview = new AIReview { ProblemId = problem.Id, PathToCPPFile = pathToSource, ProblemStatus = problem.Status };

            AIReviewValidator validator = new AIReviewValidator();
            ValidationResult result = validator.Validate(currentReview);

            try
            {
                string userSource = File.ReadAllText(Path.GetFullPath(pathToSource));
                currentReview.Review = _pythonAIService.ReviewProblem(problem.Statement!, userSource, problem.Status == ProblemStatus.Solved) ?? "Failed to generate review";

                _logger.LogInformation("Successfully created AI Review ({ReviewId}) for problem {ProblemId}", currentReview.Id, problem.Id);

                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    _unitOfWork.AIReviewRepository.SaveToDatabase(currentReview);
                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch { await _unitOfWork.RollbackTransactionAsync(); }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AI review for problem {ProblemId}", problem.Id);
            }

            return currentReview.Review;
        }

        /// <summary>
        /// Saves a <see cref="Problem"/> to the database.
        /// </summary>
        /// <param name="problem">The <see cref="Problem"/> that should be saved.</param>
        public async Task SaveToDatabase(Problem problem)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProblemRepository.SaveToDatabase(problem);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch { await _unitOfWork.RollbackTransactionAsync(); }
        }
    }
}
