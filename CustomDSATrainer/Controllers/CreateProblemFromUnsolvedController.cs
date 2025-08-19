using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateProblemFromUnsolvedController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        public CreateProblemFromUnsolvedController(IProblemGeneratorService problemGeneratorService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
        }

        [HttpGet]
        public IActionResult CreateProblemFromUnsolved()
        {
            Problem? problem = _problemGeneratorService.GenerateProblemFromUnsolved();

            if (problem != null)
                return Ok($"Generated problem with id {problem.Id}");
            else
                return BadRequest();
        }
    }
}
