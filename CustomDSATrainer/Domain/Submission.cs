using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain
{
    public class Submission
    {
        public int Id { get; set; }
        public required string PathToExecutable { get; set; }
        public SubmissionResult Result { get; set; }
        public float AverageExecutionTime { get; set; }
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
        public Submission() { }

        public Submission(string pathToExecutable, string pathToInput)
        {
            PathToExecutable = pathToExecutable;
        }

        /*public void SaveToDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var existingSubmission = context.Submissions.Find(this.Id);
                if (existingSubmission == null)
                {
                    context.Submissions.Add(this);
                }
                else
                {
                    context.Entry(existingSubmission).CurrentValues.SetValues(this);
                }

                context.SaveChanges();
            }
        }*/

        public void RunSumbission(string _inputs, string _outputs)
        {
            Result = SubmissionResult.Success;

            List<string> inputs = _inputs.Split('!').ToList();
            List<string> outputs = _outputs.Split('!').ToList();
            
            for (int i = 0; i < 7; i ++)
            {
                var testCase = new TestCase
                {
                    Id = 0,
                    SubmissionId = this.Id,
                    PathToExecutable = PathToExecutable,
                    Input = inputs[i],
                    ExpectedOutput = outputs[i]
                };

                TestCaseVerdict verdict = testCase.InitTestCase();
                //testCase.SaveToDatabase();
                    
                if (verdict == TestCaseVerdict.TimeLimitExceeded)
                {
                    Result = SubmissionResult.TimeLimitExceeded;
                    break;
                }

                if (verdict == TestCaseVerdict.MemoryLimitExceeded)
                {
                    Result = SubmissionResult.MemoryLimitExceeded;
                    break;
                }

                if (verdict == TestCaseVerdict.IncorrectAnswer)
                {
                  Result = SubmissionResult.Failed;
                  break;
                }
            }
        }
    }
}
