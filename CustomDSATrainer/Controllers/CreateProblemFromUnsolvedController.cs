using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller to generate a <see cref="Problem"/> from the unsolved ones.
    /// </summary>
    [ApiController]
    [Route("api/problem-from-unsolved")]
    public class CreateProblemFromUnsolvedController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        public CreateProblemFromUnsolvedController(IProblemGeneratorService problemGeneratorService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProblemFromUnsolved()
        {
            Problem? problem = await _problemGeneratorService.GenerateProblemFromUnsolved();

            if (problem != null)
                return Ok($"Generated problem with id {problem.Id}.");
            else
                return BadRequest("Could not generate a problem.");
        }
    }
}
