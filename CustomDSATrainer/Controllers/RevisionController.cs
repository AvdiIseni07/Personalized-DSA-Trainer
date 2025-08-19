using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevisionController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        private readonly IProblemService _problemService;
        public RevisionController(IProblemGeneratorService problemGeneratorService, IProblemService problemService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
            _problemService = problemService;
        }

        [HttpGet]
        public IActionResult Revision()
        {
            Problem? problem = _problemGeneratorService.Revision();

            if (problem != null)
            {
                _problemService.LoadProblem(problem.Id);

                return Ok($"Selected problem with id: {problem.Id} for revision");
            }
            return BadRequest();
        }
    }
}
