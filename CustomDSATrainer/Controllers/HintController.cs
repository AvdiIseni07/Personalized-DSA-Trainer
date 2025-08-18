using CustomDSATrainer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HintController : ControllerBase
    {
        private readonly ProblemService _problemService;

        public HintController(ProblemService problemService)
        {
            _problemService = problemService;
        }
        
        [HttpGet]
        public IActionResult LoadHint()
        {
            var currentProblem = _problemService.CurrentActiveProblem;
            if (currentProblem == null) { return BadRequest("No problem is currently loaded."); }
            if (currentProblem.Hint == null) { return BadRequest("This specific problem does not have a hint."); }

            return Ok(currentProblem.Hint);
        }
    }
}
