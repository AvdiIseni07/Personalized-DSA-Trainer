using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HintController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private CurrentActiveProblemService _currentActiveProblemService;

        public HintController(IProblemService problemService, CurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService;
            _currentActiveProblemService = currentActiveProblemService;
        }

        [HttpGet]
        public IActionResult LoadHint()
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) { return BadRequest("No problem is currently loaded."); }
            if (currentProblem.Hint == null) { return BadRequest("This specific problem does not have a hint."); }

            return Ok(currentProblem.Hint);
        }
    }
}
