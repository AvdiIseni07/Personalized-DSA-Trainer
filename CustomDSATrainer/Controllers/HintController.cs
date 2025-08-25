using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that shows the hint (<see cref="Problem.Hint"/>) for the selected <see cref="Problem"/>.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/hint")]
    [EnableRateLimiting("Fixed")]
    public class HintController : ControllerBase
    {
        private ICurrentActiveProblemService _currentActiveProblemService;

        public HintController(ICurrentActiveProblemService currentActiveProblemService)
        {
            _currentActiveProblemService = currentActiveProblemService ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null.");
        }

        [MapToApiVersion(1)]
        [HttpGet]
        public IActionResult LoadHint()
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) { return BadRequest("No problem is currently loaded."); }
            if (currentProblem.Hint.IsNullOrEmpty()) { return BadRequest("This specific problem does not have a hint."); }

            return Ok(new HintDTO
            {
                Hint = currentProblem.Hint ?? "",
                RevealedAt = DateTime.Now.Date
            });
        }
    }
}
