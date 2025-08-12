using CustomDSATrainer.Application;
using CustomDSATrainer.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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

    [ApiController]
    [Route("api/[controller]")]
    public class RevisionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Revision()
        {
            Problem? problem = ProblemGenerator.Revision();

            if (problem != null)
            {
                LoadProblemController controller = new LoadProblemController();
                controller.LoadProblem(problem.Id);

                return Ok($"Selected problem with id: {problem.Id} for revision ");
            }
            return BadRequest();
        }
    }
}
