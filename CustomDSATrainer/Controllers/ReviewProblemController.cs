using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that generates an <see cref="AIReview"/> for a user-generated source code.
    /// </summary>
    [ApiController]
    [Route("api/review")]
    public class ReviewProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private ICurrentActiveProblemService _currentActiveProblemService;
        private readonly IUnitOfWork _unitOfWork;
        public ReviewProblemController(IProblemService problemService, ICurrentActiveProblemService currentActiveProblemService, IUnitOfWork unitOfWork)
        {
            _problemService = problemService                            ?? throw new ArgumentNullException(nameof(problemService), "ProblemService cannot be null");
            _currentActiveProblemService = currentActiveProblemService  ?? throw new ArgumentNullException(nameof(currentActiveProblemService), "CurrentActiveProblemService");
            _unitOfWork = unitOfWork                                    ?? throw new ArgumentNullException(nameof(_unitOfWork), "UnitOfWork cannot be null.");
        }

        [HttpPost("{SourceCodePath}")]
        public async Task<IActionResult> ReviewUnsolvedProblem(string SourceCodePath)
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) return BadRequest("There is no problem loaded"); 

            string review = await _problemService.AiReview(currentProblem, SourceCodePath);

            if (review != null) return Ok(review);
            return BadRequest("Error generating review");
        }
    }
}
