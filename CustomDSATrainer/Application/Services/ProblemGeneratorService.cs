using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Validators;
using FluentValidation.Results;
using System.CodeDom;

namespace CustomDSATrainer.Application.Services
{
    public class ProblemGeneratorService : IProblemGeneratorService
    {
        private readonly IPythonAIService _pythonAIService;
        private readonly IProblemService _problemService;
        private readonly IProblemRepository _problemRepository;

        public ProblemGeneratorService(IPythonAIService pythonAIService, IProblemService problemService, IProblemRepository problemRepository)
        {
            _pythonAIService = pythonAIService      ?? throw new ArgumentNullException(nameof(pythonAIService), "PythonAIService cannot be null.");
            _problemService = problemService        ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
            _problemRepository = problemRepository  ?? throw new ArgumentNullException(nameof(problemRepository), "ProblemRepository cannot be null");
        }

        private Problem InitProblem(List<string> problemData)
        {
            string statement = problemData[0];
            string title = statement.Split('\n')[0];
            string inputs = problemData[1];
            string outputs = problemData[2];
            string hint = problemData[3];
            string categories = problemData[4];
            string difficulty = problemData[5];

            title = title.Remove(0, 7);
            statement = statement.Remove(0, 7);

            Problem problem = new Problem
            {
                Id = 0,
                Title = title,
                Statement = statement,
                Difficulty = difficulty,
                Categories = categories,
                Hint = hint,
                Inputs = inputs,
                Outputs = outputs
            };

            ProblemValidator validator = new ProblemValidator();
            ValidationResult result = validator.Validate(problem);

            if (!result.IsValid)
            {
                throw new Exception(result.Errors.ToArray().ToString());
            }

            _problemService.SaveToDatabase(problem);

            return problem;
        }
        public Problem GenerateFromPrompt(string categories, string difficulty)
        {
            List<string> problemData = _pythonAIService.GenerateProblemFromPrompt(categories, difficulty);

            return InitProblem(problemData);
        }

        public async Task<Problem?> GenerateProblemFromUnsolved()
        {
            Problem? generatedProblem = null;
            Tuple<string, string> data = await _problemRepository.GetUnsolvedData();

            List<string> problemData = _pythonAIService.GenerateProblemFromUnsolved(data.Item1, data.Item2);
            generatedProblem = InitProblem(problemData);

            return generatedProblem;
        }

        public async Task<Problem?> Revision()
        {
            Problem? generatedProblem = await _problemRepository.GetRevision();

            return generatedProblem;
        }

        public async Task<Problem?> RevisionWithCategories(string categories)
        {
            Problem? generatedProblem = await _problemRepository.GetRevisionWithCategories(categories);

            return generatedProblem;
        }
    }
}
