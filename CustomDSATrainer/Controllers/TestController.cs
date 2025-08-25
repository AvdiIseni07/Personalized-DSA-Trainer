using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Permissions;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that makes a <see cref="Submission"/> for the loaded <see cref="Problem"/>.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/submit")]
    //[EnableRateLimiting("Fixed")]
    public class TestController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private ICurrentActiveProblemService _currentActiveProblemService;
        public TestController(IProblemService problemService, ICurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService                            ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
            _currentActiveProblemService = currentActiveProblemService  ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null."); 
        }

        [[MapToApiVersion(1)]]
        [HttpPost("{ExePath}")]
        public IActionResult GetExePath(string ExePath)
        {
            if (_currentActiveProblemService.CurrentProblem == null) return BadRequest("There is no problem loaded.");
            _problemService.SubmitProblem(_currentActiveProblemService.CurrentProblem, ExePath);

            return Ok(new TestDTO
            {
                Message = (_currentActiveProblemService.CurrentProblem.Status == ProblemStatus.Solved) ? "Problem solved succesfully." : "Problem was not solved."
            });
        }
    }
}
