using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.UnitOfWork;
using CustomDSATrainer.Domain.Validators;
using FluentValidation.Results;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality for generating problems from prompt, from unsolved and for revision with or without categories.
    /// </summary>
    public class ProblemGeneratorService : IProblemGeneratorService
    {
        private readonly IPythonAIService _pythonAIService;
        private readonly IProblemService _problemService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemGeneratorService> _logger;

        private const int TITLE_PREFIX_LENGTH = 7;
        public ProblemGeneratorService(IPythonAIService pythonAIService, IProblemService problemService, IUnitOfWork unitOfWork, ILogger<ProblemGeneratorService> logger)
        {
            _pythonAIService = pythonAIService      ?? throw new ArgumentNullException(nameof(pythonAIService), "PythonAIService cannot be null.");
            _problemService = problemService        ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
            _unitOfWork = unitOfWork                ?? throw new ArgumentNullException(nameof(unitOfWork), "UnitOfWork cannot be null");
            _logger = logger                        ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Initialises a new <see cref="Problem"/> based on the gived problem data.
        /// </summary>
        /// <param name="problemData">Cointains all the data needed for the problem:
        /// <list type="bullet">
        /// <item>[0] = Title and statement.</item>
        /// <item>[1] = Problem inputs</item>
        /// <item>[2] = Expected outputs</item>
        /// <item>[3] = Hint</item>
        /// <item>[4] = Categories</item>
        /// <item>[5] = Difficulty</item>
        /// </list>                                              
        /// </param>
        /// <returns>Returns the generated <see cref="Problem"/> if generation is succesful.</returns>
        /// <exception cref="Exception">The validator has deemed that it has not been initialized correctly</exception>
        private Problem InitProblem(List<string> problemData)
        {
            string statement = problemData[0];
            string title = statement.Split('\n')[0];
            string inputs = problemData[1];
            string outputs = problemData[2];
            string hint = problemData[3];
            string categories = problemData[4];
            string difficulty = problemData[5];

            title = title.Remove(0, TITLE_PREFIX_LENGTH);
            statement = statement.Remove(0, TITLE_PREFIX_LENGTH);

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

        /// <summary>
        /// Generates a <see cref="Problem"/> based on the given prompts from the user.
        /// </summary>
        /// <param name="categories">The categories that the problem shuld have.</param>
        /// <param name="difficulty">The difficilty of the problem.</param>
        /// <returns>The generated <see cref="Problem"/>, if generated successfully.</returns>
        public Problem GenerateFromPrompt(string categories, string difficulty)
        {
            _logger.LogInformation("Generating from promp: Categories = {Cat}    Difficulty: {Diff}", categories, difficulty);
            List<string> problemData = _pythonAIService.GenerateProblemFromPrompt(categories, difficulty);

            return InitProblem(problemData);
        }

        /// <summary>
        /// Goes through all the unsolved problems of the user and gathers the categories and difficulties.
        /// It then picks at random what three categories to use and what difficulty should the problem be.
        /// It then generates a problem based on these picks.
        /// </summary>
        /// <returns>The generated <see cref="Problem"/>, if generated successfully</returns>
        public async Task<Problem?> GenerateProblemFromUnsolved()
        {
            _logger.LogInformation("Generating problem from unsolved");
            Problem? generatedProblem = null;
            Tuple<string, string> data = await _unitOfWork.ProblemRepository.GetUnsolvedData();

            List<string> problemData = _pythonAIService.GenerateProblemFromUnsolved(data.Item1, data.Item2);
            generatedProblem = InitProblem(problemData);

            return generatedProblem;
        }

        /// <summary>
        /// It picks a random <see cref="Problem"/> that the user has already solved.
        /// It then sends it back to the user to be solved again.
        /// </summary>
        /// <returns>The selected <see cref="Problem"/>, if the selection process is successful</returns>
        public async Task<Problem?> Revision()
        {
            _logger.LogInformation("Getting problem for revision");
            Problem? generatedProblem = await _unitOfWork.ProblemRepository.GetRevision();

            return generatedProblem;
        }

        /// <summary>
        /// It does the same as <see cref="Revision">, except that it picks at random from only those solved problems that have the specified categories.
        /// </summary>
        /// <param name="categories">The categories that the already solved problems should contain</param>
        /// <returns>The selected <see cref="Problem"/>, if the selection process is successful</returns>
        public async Task<Problem?> RevisionWithCategories(string categories)
        {
            Problem? generatedProblem = await _unitOfWork.ProblemRepository.GetRevisionWithCategories(categories);

            return generatedProblem;
        }
    }
}
