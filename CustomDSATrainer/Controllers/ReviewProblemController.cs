using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;

namespace CustomDSATrainer.Controllers
{
    /// <summary>
    /// A controller that generates an <see cref="AIReview"/> for a user-generated source code.
    /// </summary>
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/review")]
    [EnableRateLimiting("Fixed")]
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

        [MapToApiVersion(1)]
        [HttpPost("{SourceCodePath}")]
        public async Task<IActionResult> ReviewUnsolvedProblem(string SourceCodePath)
        {
            var currentProblem = _currentActiveProblemService.CurrentProblem;
            if (currentProblem == null) return BadRequest("There is no problem loaded"); 

            string review = await _problemService.AiReview(currentProblem, SourceCodePath) ?? "";

            if (!review.IsNullOrEmpty()) return Ok(new AIReviewDTO
            {
                Review = review,
                CreatedAt = DateTime.Now.Date
            });

            return BadRequest("Error generating review");
        }
    }
}
