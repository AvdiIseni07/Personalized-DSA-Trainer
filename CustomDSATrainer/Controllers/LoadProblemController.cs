using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoadProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        private readonly CurrentActiveProblemService _currentActiveProblemService;
        public LoadProblemController(IProblemService problemService, IDbContextFactory<ProjectDbContext> contextFactory, CurrentActiveProblemService currentActiveProblemService)
        {
            _problemService = problemService;
            _contextFactory = contextFactory;
            _currentActiveProblemService = currentActiveProblemService;
        }

        [HttpGet("{ProblemId}")]
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

            return NotFound($"Problem with id: {ProblemId} was not found");
        }
    }
}
