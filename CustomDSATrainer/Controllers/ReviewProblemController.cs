using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private CurrentActiveProblemService _currentActiveProblemService;
        public ReviewProblemController(IProblemService problemService, CurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null");
            _currentActiveProblemService = currentActiveProblemService;
        }


        [HttpGet("{SourceCodePath}")]
        public IActionResult ReviewUnsolvedProblem(string SourceCodePath)
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) { return BadRequest("There is no problem loaded"); }

            string? review = _problemService.AiReview(currentProblem, SourceCodePath);

            if (review != null)
            {
                return Ok(review);
            }
            return BadRequest();
        }
    }
}
