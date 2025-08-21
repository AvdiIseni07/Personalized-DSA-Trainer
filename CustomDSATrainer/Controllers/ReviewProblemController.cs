using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that generates an <see cref="AIReview"/> for a user-generated source code.
    /// </summary>
    [ApiController]
    [Route("api/review")]
    public class ReviewProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private ICurrentActiveProblemService _currentActiveProblemService;
        public ReviewProblemController(IProblemService problemService, ICurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService                            ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null");
            _currentActiveProblemService = currentActiveProblemService  ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService");
        }

        [HttpPost("{SourceCodePath}")]
        public IActionResult ReviewUnsolvedProblem(string SourceCodePath)
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) return BadRequest("There is no problem loaded"); 

            string? review = _problemService.AiReview(currentProblem, SourceCodePath);

            if (review != null) return Ok(review);
            return BadRequest("Error generating review");
        }
    }
}
