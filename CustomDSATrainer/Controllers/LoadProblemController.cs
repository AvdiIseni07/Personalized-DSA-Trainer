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

                    for (int i = 1; i <= 7; i ++)
                    {
                        var currentInput = Directory.GetFiles("AIService/Task/Inputs", $"{i.ToString()}.txt", SearchOption.AllDirectories)[0];
                        var currentOutput = Directory.GetFiles("AIService/Task/Outputs", $"{i.ToString()}.txt", SearchOption.AllDirectories)[0];

                        if (inputs[i - 1].StartsWith('\n'))
                            inputs[i - 1].Remove(0, 1);

                        if (outputs[i - 1].StartsWith('\n'))
                            outputs[i - 1].Remove(0, 1);

                        System.IO.File.WriteAllText(currentInput, inputs[i - 1]);
                        System.IO.File.WriteAllText(currentOutput, outputs[i - 1]);
                    }

                    return Ok($"The following problem was retrieved succesfully.\n\n{SharedValues.CurrentActiveProblem.Statement}");
                }
            }

            return NotFound($"Problem with id: {ProblemId} was not found");
        }
    }
}
