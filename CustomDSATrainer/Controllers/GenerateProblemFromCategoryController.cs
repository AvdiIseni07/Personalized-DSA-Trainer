using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that generates a <see cref="Problem"/> based on the given categories and difficulty.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateProblemFromCategoryController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        public GenerateProblemFromCategoryController(IProblemGeneratorService problemGeneratorService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
        }

        [HttpPost("{Categories}/{Difficulty}")]
        public IActionResult CreateProblemFromCategory(string Categories, string Difficulty)
        {
            Problem problem = _problemGeneratorService.GenerateFromPrompt(Categories, Difficulty);

            return Ok($"Generated problem with id {problem.Id}");
        }
    }
}
