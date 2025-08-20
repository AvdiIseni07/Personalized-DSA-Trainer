using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/revision-with-categories")]
    public class RevisionWithCategoriesController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        private readonly IProblemService _problemService;
        public RevisionWithCategoriesController(IProblemGeneratorService problemGeneratorService, IProblemService problemService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
            _problemService = problemService;
        }

        [HttpPost("{Categories}")]
        public async Task<IActionResult> RevisionWithCategories(string Categories)
        {
            Problem? problem = await _problemGeneratorService.RevisionWithCategories(Categories);

            if (problem != null)
            {
                await _problemService.LoadProblem(problem.Id);

                return Ok($"Selected problem with id: {problem.Id} for revision");
            }
            else
                return NotFound("Error finding a problem");
        }
    }
}
