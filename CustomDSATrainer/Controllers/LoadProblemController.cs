using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that loads a <see cref="Problem"/> based on the given ID.
    /// </summary>
    [ApiController]
    [Route("api/load-problem")]
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

        [HttpPost("{ProblemId}")]
        public IActionResult LoadProblem(int ProblemId)
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem != null && currentProblem.Status == ProblemStatus.Solving)
            {
                currentProblem.Status = ProblemStatus.WasSolving;
                _problemService.SaveToDatabase(currentProblem);
            }

            _problemService.LoadProblem(ProblemId);

            if (_currentActiveProblemService.CurrentProblem != null)
                return Ok($"The following problem was retrieved succesfully.\n\n{_currentActiveProblemService.CurrentProblem.Statement}");

            return NotFound($"Problem with id: {ProblemId} was not found.");
        }
    }
}
