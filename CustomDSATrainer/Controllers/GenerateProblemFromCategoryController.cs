using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that generates a <see cref="Problem"/> based on the given categories and difficulty.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/Generate-from-prompt")]
    [EnableRateLimiting("Fixed")]
    public class GenerateProblemFromCategoryController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        public GenerateProblemFromCategoryController(IProblemGeneratorService problemGeneratorService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
        }

        [MapToApiVersion(1)]
        [HttpPost("{Categories}/{Difficulty}")]
        public IActionResult CreateProblemFromCategory(string Categories, string Difficulty)
        {
            Problem? problem = _problemGeneratorService.GenerateFromPrompt(Categories, Difficulty);

            if (problem != null)
                return Ok(new ProblemDTO
                {
                    Id = problem.Id,
                    Title = problem.Title ?? "",
                    Statement = problem.Statement ?? "",
                    CreatedAt = DateTime.Now.Date,
                });
            else
                return BadRequest("Problem could not be generated.");
        }
    }
}
