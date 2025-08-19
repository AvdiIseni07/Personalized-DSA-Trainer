using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevisionWithCategoriesController : ControllerBase
    {
        private readonly IProblemGeneratorService _problemGeneratorService;
        public RevisionWithCategoriesController(IProblemGeneratorService problemGeneratorService)
        {
            _problemGeneratorService = problemGeneratorService ?? throw new ArgumentNullException(nameof(problemGeneratorService), "ProblemGeneratorService cannot be null.");
        }

        [HttpGet("{Categories}")]
        public IActionResult RevisionWithCategories(string Categories)
        {
            Problem? problem = _problemGeneratorService.RevisionWithCategories(Categories);

            if (problem != null)
            {
                // LoadProblemController loadProblemController = new LoadProblemController();
                // loadProblemController.LoadProblem(problem.Id);

                return Ok($"Selected problem with id: {problem.Id} for revision");
            }
            else
                return BadRequest();
        }
    }
}
