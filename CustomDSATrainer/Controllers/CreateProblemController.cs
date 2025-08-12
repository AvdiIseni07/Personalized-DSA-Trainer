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
        public IActionResult CreateProblemFromCategory(string Categories)
        {
            Problem problem = ProblemGenerator.GenerateFromPrompt(Categories);

            return Ok($"Generated problem with id {problem.Id}");
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FromUnsolvedController : ControllerBase
    {
        [HttpGet]
        public IActionResult CreateProblemFromUnsolved()
        {
            Problem? problem = ProblemGenerator.GenerateProblemFromUnsolved();

            if (problem != null)
                return Ok($"Generated problem with id {problem.Id}");
            else
                return BadRequest();
        }
    }
}
