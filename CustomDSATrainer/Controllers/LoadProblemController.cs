using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoadProblemController : ControllerBase
    {
        private readonly ProblemService _problemService;
        private readonly DatabaseService _databaseService;
        public LoadProblemController(ProblemService problemService, DatabaseService databaseService)
        {
            _problemService = problemService;
            _databaseService = databaseService;
        }

        public LoadProblemController() { }

        [HttpGet("{ProblemId}")]
        public IActionResult LoadProblem(int ProblemId)
        {
            var currentProblem = _problemService.CurrentActiveProblem;
            if (currentProblem != null && currentProblem.Status == ProblemStatus.Solving)
            {
                currentProblem.Status = ProblemStatus.WasSolving;
                //currentProblem.SaveToDatabase();
            }

            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(_databaseService.GetConnectionString());
            
            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var problem = context.Problem.FirstOrDefault(p => p.Id == ProblemId);

                if (problem != null)
                {
                    currentProblem = problem;

                    if (currentProblem.Status == ProblemStatus.Unsolved)
                    {
                        currentProblem.Status = ProblemStatus.Solving;
                        //currentProblem.SaveToDatabaes();
                    }

                    string[] inputs = problem.Inputs.Split('!');
                    string[] outputs = problem.Outputs.Split('!');

                    return Ok($"The following problem was retrieved succesfully.\n\n{currentProblem.Statement}");
                }
            }

            return NotFound($"Problem with id: {ProblemId} was not found");
        }
    }
}
