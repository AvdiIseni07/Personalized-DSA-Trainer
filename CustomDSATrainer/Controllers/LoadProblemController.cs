using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that loads a <see cref="Problem"/> based on the given ID.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/load-problem")]
    [EnableRateLimiting("Fixed")]
    public class LoadProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        private readonly ICurrentActiveProblemService _currentActiveProblemService;
        public LoadProblemController(IProblemService problemService, IDbContextFactory<ProjectDbContext> contextFactory, ICurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService                           ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null.");
            _contextFactory = contextFactory                           ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null.");
            _currentActiveProblemService = currentActiveProblemService ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService cannot be null.");
        }

        [MapToApiVersion(1)]
        [HttpPost("{ProblemId}")]
        public IActionResult LoadProblem(int ProblemId)
        {
            _problemService.LoadProblem(ProblemId);

            if (_currentActiveProblemService.CurrentProblem != null)
                return Ok(new ProblemDTO
                {
                    Id = ProblemId,
                    Title = _currentActiveProblemService.CurrentProblem.Title ?? "",
                    Statement = _currentActiveProblemService.CurrentProblem.Statement ?? "",
                    CreatedAt = DateTime.Now
                });

            return NotFound($"Problem with id: {ProblemId} was not found.");
        }
    }
}
