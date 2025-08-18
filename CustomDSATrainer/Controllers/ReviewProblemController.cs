using CustomDSATrainer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewProblemController : ControllerBase
    {
        private readonly ProblemService _problemService;
        public ReviewProblemController(ProblemService problemService)
        {
            _problemService = problemService;
        }


        [HttpGet("{SourceCodePath}")]
        public IActionResult ReviewUnsolvedProblem(string SourceCodePath)
        {
            var currentProblem = _problemService.CurrentActiveProblem;
            if (currentProblem == null) { return BadRequest("There is no problem loaded"); }

            string? review = currentProblem.AiReview(SourceCodePath);

            if (review != null)
            {
                return Ok(review);
            }
            return BadRequest();
        }
    }
}
