using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Validators;
using FluentValidation.Results;

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
        private readonly IProblemService _problemService;
        private readonly IPythonAIService _pythonAIService;
        private ICurrentActiveProblemService _currentActiveProblemService;

        private readonly IProblemRepository _problemRepository;
        private readonly IUserProgressRepository _userProgressRepository;
        private readonly IAIReviewRepository _aiReviewRepository;

        private readonly ILogger<ProblemService> _logger;

        public ProblemService(
            ISubmissionService submissionService, IProblemService problemService,ICurrentActiveProblemService currentActiveProblemService, IPythonAIService pythonAIService, 
            IProblemRepository problemRepository, IUserProgressRepository userProgressRepository, IAIReviewRepository aIReviewRepository, ILogger<ProblemService> logger)
        {
            _submissionService = submissionService                      ?? throw new ArgumentNullException(nameof(submissionService), "Submission service cannot be null.");
            _problemService = problemService                            ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null");
            _currentActiveProblemService = currentActiveProblemService  ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null.");
            _pythonAIService = pythonAIService                          ?? throw new ArgumentNullException(nameof(pythonAIService), "PythonAIService cannot be null.");
            _problemRepository = problemRepository                      ?? throw new ArgumentNullException(nameof(problemRepository), "ProblemRepository cannot be null.");
            _userProgressRepository = userProgressRepository            ?? throw new ArgumentNullException(nameof(userProgressRepository), "UserProgressRepository cannot be null");
            _aiReviewRepository = aIReviewRepository                    ?? throw new ArgumentNullException(nameof(aIReviewRepository), "AIReviewRepository cannot be null"); 
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

            Submission submission = new Submission { ProblemId = problem.Id, Id = 0, PathToExecutable = pathToExe };

            SubmissionValidator validator = new SubmissionValidator();
            ValidationResult validationResult = validator.Validate(submission);

            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.Errors.ToArray().ToString());
            }
            
            _submissionService.RunSumbission(submission, problem.Inputs, problem.Outputs);
            _submissionService.SaveToDatabase(submission);

            _userProgressRepository.UpdateProblemData(problem, submission);
        }
        
        /// <summary>
        /// Gets a <see cref="Problem"/> from the database based on the given id and loads it.
        /// If it is loaded successfully the user will be able to submit an executable for this problem.
        /// </summary>
        /// <param name="id">The ID of the problem (<see cref="Problem.Id"/>) that needs to be loaded.</param>
        /// <returns>Whether the <see cref="Problem"/> has been loaded successfully.</returns>
        public async Task<bool> LoadProblem(int id)
        {
            Problem? currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem != null && currentProblem.Status == ProblemStatus.Solving)
            {
                currentProblem.Status = ProblemStatus.WasSolving;
                _problemService.SaveToDatabase(currentProblem);
            }

            currentProblem = await _problemRepository.GetFromId(id);
            _currentActiveProblemService.CurrentProblem = currentProblem;

            if (currentProblem != null)
            {
                if (currentProblem.Status == ProblemStatus.Unsolved)
                {
                    currentProblem.Status = ProblemStatus.Solving;
                    SaveToDatabase(currentProblem);
                }

                string[] inputs = currentProblem.Inputs.Split('!');
                string[] outputs = currentProblem.Outputs.Split('!');

                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates an <see cref="AIReview"/> for a given problem. 
        /// It will analyze the user source code and it will give an appropriate review.
        /// </summary>
        /// <param name="problem">The selected <see cref="Problem"/>.</param>
        /// <param name="pathToSource">The source code that should be reviewed.</param>
        /// <returns>The generated review.</returns>
        public string? AiReview(Problem problem, string pathToSource)
        {
            if (!File.Exists(pathToSource))
                return null;

            AIReview currentReview = new AIReview { ProblemId = problem.Id, PathToCPPFile = pathToSource, ProblemStatus = problem.Status };

            AIReviewValidator validator = new AIReviewValidator();
            ValidationResult result = validator.Validate(currentReview);

            try
            {
                string userSource = File.ReadAllText(Path.GetFullPath(pathToSource));
                currentReview.Review = _pythonAIService.ReviewProblem(problem.Statement!, userSource, problem.Status == ProblemStatus.Solved);

                _aiReviewRepository.SaveToDatabase(currentReview);
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
        public void SaveToDatabase(Problem problem)
        {
            _problemRepository.SaveToDatabase(problem);
        }
    }
}
