using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;


namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller to generate a <see cref="Problem"/> from the unsolved ones.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/problem-from-unsolved")]
    public class CreateProblemFromUnsolvedController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        public CreateProblemFromUnsolvedController(IProblemGeneratorService problemGeneratorService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
        }

        [MapToApiVersion(1)]
        [HttpPost]
        public async Task<IActionResult> CreateProblemFromUnsolved()
        {
            Problem? problem = await _problemGeneratorService.GenerateProblemFromUnsolved();

            if (problem == null)
                return BadRequest();
            else
                return Ok(new ProblemDTO
                {
                    Id = problem.Id,
                    Title = problem.Title ?? "",
                    Statement = problem.Statement ?? "",
                    CreatedAt = DateTime.Now,
                });
        }
    }
}
