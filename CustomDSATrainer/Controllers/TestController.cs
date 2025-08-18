using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ProblemService _problemService;
        public TestController(ProblemService problemService)
        {
            _problemService = problemService;
        }

        [HttpGet("{ExePath}")]
        public IActionResult GetExePath(string ExePath)
        {
            var currentProblem = _problemService.CurrentActiveProblem;
            currentProblem.Submit(ExePath);

            string returnMessage = (currentProblem.Status == ProblemStatus.Solved) ? "Problem solved succesfully." : "Problem was not solved.";

            return Ok(returnMessage);
        }
    }
}
