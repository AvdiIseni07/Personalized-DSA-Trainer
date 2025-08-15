using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("{ExePath}")]
        public IActionResult GetExePath(string ExePath)
        {
            SharedValues.CurrentActiveProblem.Submit(ExePath);

            string returnMessage = (SharedValues.CurrentActiveProblem.Status == ProblemStatus.Solved) ? "Problem solved succesfully." : "Problem was not solved.";

            return Ok(returnMessage);
        }
    }
}
