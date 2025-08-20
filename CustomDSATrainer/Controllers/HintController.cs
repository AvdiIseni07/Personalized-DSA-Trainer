using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/hint")]
    public class HintController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private ICurrentActiveProblemService _currentActiveProblemService;

        public HintController(IProblemService problemService, ICurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService                           ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
            _currentActiveProblemService = currentActiveProblemService ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null.");
        }

        [HttpGet]
        public IActionResult LoadHint()
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) { return BadRequest("No problem is currently loaded."); }
            if (currentProblem.Hint.IsNullOrEmpty()) { return BadRequest("This specific problem does not have a hint."); }

            return Ok($"Hint for selected problem: {currentProblem.Hint}");
        }
    }
}
