using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/revision")]
    public class RevisionController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        private readonly IProblemService _problemService;
        public RevisionController(IProblemGeneratorService problemGeneratorService, IProblemService problemService)
        {
            _problemGeneratorService = problemGeneratorService  ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
            _problemService = problemService                    ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
        }

        [HttpPost]
        public async Task<IActionResult> Revision()
        {
            Problem? problem = await _problemGeneratorService.Revision();

            if (problem != null)
            {
                await _problemService.LoadProblem(problem.Id);

                return Ok($"Selected problem with id: {problem.Id} for revision");
            }
            return NotFound("Error finding a problem");
        }
    }
}
