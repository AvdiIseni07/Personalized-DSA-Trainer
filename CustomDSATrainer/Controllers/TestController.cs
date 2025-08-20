using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    
    [ApiController]
    [Route("api/submit")]
    public class TestController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private ICurrentActiveProblemService _currentActiveProblemService;
        public TestController(IProblemService problemService, ICurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService                            ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
            _currentActiveProblemService = currentActiveProblemService  ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null."); 
        }

        [HttpPost("{ExePath}")]
        public IActionResult GetExePath(string ExePath)
        {
            if (_currentActiveProblemService.CurrentProblem == null) return BadRequest("There is no problem loaded.");
            _problemService.SubmitProblem(_currentActiveProblemService.CurrentProblem, ExePath);

            string returnMessage = (_currentActiveProblemService.CurrentProblem.Status == ProblemStatus.Solved) ? "Problem solved succesfully." : "Problem was not solved.";
            return Ok(returnMessage);
        }
    }
}
