using CustomDSATrainer.Application;
using CustomDSATrainer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("{Categories}")]
        public IActionResult TestProblemFromCategory(string Categories)
        {
            ProblemGenerator.categories = Categories;
            Problem problem = ProblemGenerator.GenerateFromPrompt();

            return Ok($"Generated problem with id {problem.Id}");
        }
    }
}
