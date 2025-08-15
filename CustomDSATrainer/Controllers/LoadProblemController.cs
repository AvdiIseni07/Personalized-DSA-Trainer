using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoadProblemController : ControllerBase
    {
        [HttpGet("{ProblemId}")]
        public IActionResult LoadProblem(int ProblemId)
        {
            if (SharedValues.CurrentActiveProblem != null && SharedValues.CurrentActiveProblem.Status == ProblemStatus.Solving)
            {
                SharedValues.CurrentActiveProblem.Status = ProblemStatus.WasSolving;
                SharedValues.CurrentActiveProblem.SaveToDatabase();
            }

            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);
            
            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var problem = context.Problem.FirstOrDefault(p => p.Id == ProblemId);

                if (problem != null)
                {
                    SharedValues.CurrentActiveProblem = problem;

                    if (SharedValues.CurrentActiveProblem.Status == ProblemStatus.Unsolved)
                    {
                        SharedValues.CurrentActiveProblem.Status = ProblemStatus.Solving;
                        SharedValues.CurrentActiveProblem.SaveToDatabase();
                    }

                    string[] inputs = problem.Inputs.Split('!');
                    string[] outputs = problem.Outputs.Split('!');

                    return Ok($"The following problem was retrieved succesfully.\n\n{SharedValues.CurrentActiveProblem.Statement}");
                }
            }

            return NotFound($"Problem with id: {ProblemId} was not found");
        }
    }
}
