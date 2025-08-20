using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Validators;
using FluentValidation.Results;
using System.Runtime.CompilerServices;

namespace CustomDSATrainer.Application.Services
{
    public class ProblemService : IProblemService
    {
        private ICurrentActiveProblemService _currentActiveProblem;

        private readonly ISubmissionService _submissionService;
        private readonly IPythonAIService _pythonAIService;

        private readonly IProblemRepository _problemRepository;
        private readonly IUserProgressRepository _userProgressRepository;
        private readonly IAIReviewRepository _aiReviewRepository;

        private readonly ILogger<ProblemService> _logger;

        public ProblemService(
            ISubmissionService submissionService, ICurrentActiveProblemService currentActiveProblemService, IPythonAIService pythonAIService, 
            IProblemRepository problemRepository, IUserProgressRepository userProgressRepository, IAIReviewRepository aIReviewRepository, ILogger<ProblemService> logger)
        {
            _submissionService = submissionService              ?? throw new ArgumentNullException(nameof(submissionService), "Submission service cannot be null.");
            _currentActiveProblem = currentActiveProblemService ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "Current active problem service cannot be null.");
            _pythonAIService = pythonAIService                  ?? throw new ArgumentNullException(nameof(pythonAIService), "PythonAIService cannot be null.");
            _problemRepository = problemRepository              ?? throw new ArgumentNullException(nameof(problemRepository), "ProblemRepository cannot be null.");
            _userProgressRepository = userProgressRepository    ?? throw new ArgumentNullException(nameof(userProgressRepository), "UserProgressRepository cannot be null");
            _aiReviewRepository = aIReviewRepository            ?? throw new ArgumentNullException(nameof(aIReviewRepository), "AIReviewRepository cannot be null"); 
            _logger = logger                                    ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }

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

        public async Task<bool> LoadProblem(int id)
        {
            Problem? problem = await _problemRepository.GetFromId(id);
            _currentActiveProblem.CurrentProblem = problem;

            if (problem != null)
            {
                if (problem.Status == ProblemStatus.Unsolved)
                {
                    problem.Status = ProblemStatus.Solving;
                    SaveToDatabase(problem);
                }

                string[] inputs = problem.Inputs.Split('!');
                string[] outputs = problem.Outputs.Split('!');

                return true;
            }

            return false;
        }
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

        public void SaveToDatabase(Problem problem)
        {
            _problemRepository.SaveToDatabase(problem);
        }
    }
}
