using CustomDSATrainer.Application;
using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewProblemController : ControllerBase
    {
        [HttpGet("{SourceCodePath}")]
        public IActionResult ReviewUnsolvedProblem(string SourceCodePath)
        {
            if (SharedValues.CurrentActiveProblem == null) { return BadRequest("There is no problem loaded"); }

            string? review = SharedValues.CurrentActiveProblem.AiReview(SourceCodePath);

            if (review != null)
            {
                return Ok(review);
            }
            return BadRequest();
        }
    }
}
