using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private CurrentActiveProblemService _currentActiveProblemService;
        public TestController(IProblemService problemService, CurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService;
            _currentActiveProblemService = currentActiveProblemService;
        }

        [HttpGet("{ExePath}")]
        public IActionResult GetExePath(string ExePath)
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) {return BadRequest("There is no problem loaded.");}
            _problemService.SubmitProblem(currentProblem, ExePath);

            string returnMessage = (currentProblem.Status == ProblemStatus.Solved) ? "Problem solved succesfully." : "Problem was not solved.";

            return Ok(returnMessage);
        }
    }
}
