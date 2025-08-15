using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HintController : ControllerBase
    {
        [HttpGet]
        public IActionResult LoadHint()
        {
            if (SharedValues.CurrentActiveProblem == null) { return BadRequest("No problem is currently loaded."); }
            if (SharedValues.CurrentActiveProblem.Hint == null) { return BadRequest("This specific problem does not have a hint."); }

            return Ok(SharedValues.CurrentActiveProblem.Hint);
        }
    }
}
