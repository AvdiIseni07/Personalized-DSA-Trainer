using CustomDSATrainer.Application;
using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
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
