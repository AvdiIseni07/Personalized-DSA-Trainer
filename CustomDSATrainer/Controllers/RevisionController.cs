using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that selects a random <see cref="Problem"/> for revision.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/revision")]
    [EnableRateLimiting("Fixed")]
    public class RevisionController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        private readonly IProblemService _problemService;
        public RevisionController(IProblemGeneratorService problemGeneratorService, IProblemService problemService)
        {
            _problemGeneratorService = problemGeneratorService  ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
            _problemService = problemService                    ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
        }

        [MapToApiVersion(1)]
        [HttpPost]
        public async Task<IActionResult> Revision()
        {
            Problem? problem = await _problemGeneratorService.Revision();

            if (problem != null)
            {
                await _problemService.LoadProblem(problem.Id);

                return Ok(new RevisionDTO
                {
                    ProblemId = problem.Id,
                    ProblemTitle = problem.Title ?? "",
                    ProblemStatement = problem.Statement ?? "",
                    SelectedAt = DateTime.Now.Date
                });
            }
            return NotFound("Error finding a problem");
        }
    }
}
