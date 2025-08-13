using CustomDSATrainer.Application;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace CustomDSATrainer.Domain
{
    public class Submission
    {
        public int Id { get; set; }
        public required string PathToExecutable { get; set; }
        private string PathToInput { get; set; } = "AIService/Task/Inputs";
        private string PathToOutput { get; set; } = "AIService/Task/Outputs";
        public SubmissionResult Result { get; set; }
        public float AverageExecutionTime { get; set; }
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
        public Submission() { }

        public Submission(string pathToExecutable, string pathToInput)
        {
            PathToExecutable = pathToExecutable;
            PathToInput = pathToInput;
        }

        public void SaveToDatabase()
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
        }

        public void RunSumbission()
        {
            Result = SubmissionResult.Success;

            for (int i = 1; i <= 7; i ++)
            {
                string[] currentInput = Directory.GetFiles(PathToInput, $"{i.ToString()}.txt", SearchOption.AllDirectories);
                string[] currentOutput = Directory.GetFiles(PathToOutput, $"{i.ToString()}.txt", SearchOption.AllDirectories);

                if (currentInput.Length == 0)
                {
                    Result = SubmissionResult.FailedToLocateInput;
                    return; 
                }

                if (currentOutput.Length == 0)
                {
                    Result = SubmissionResult.FailedToLocateOutput;
                    return;
                }

                var testCase = new TestCase
                {
                    Id = 0,
                    SubmissionId = this.Id,
                    PathToExecutable = PathToExecutable,
                    PathToInputFile = currentInput[0],
                    PathToExpectedOutputFile = currentOutput[0]
                };

                TestCaseVerdict verdict = testCase.InitTestCase();
                testCase.SaveToDatabase();
                    
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
