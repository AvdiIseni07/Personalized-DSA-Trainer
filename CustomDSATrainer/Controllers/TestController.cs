using CustomDSATrainer.Application;
using CustomDSATrainer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CustomDSATrainer.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("{ExePath}")]
        public IActionResult GetExePath(string ExePath)
        {
            return Ok(ExePath);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class categoryController : ControllerBase
    {
        [HttpGet("{Categories}")]
        public IActionResult TestProblemFromCategory(string Categories)
        {
            ProblemGenerator.categories = Categories;
            Problem problem = ProblemGenerator.GenerateFromPrompt();

            return Ok(problem);
        }
    }
}
