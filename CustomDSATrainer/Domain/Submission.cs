using CustomDSATrainer.Application;
using CustomDSATrainer.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
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
        public Submission() { }

        public Submission(string pathToExecutable, string pathToInput)
        {
            PathToExecutable = pathToExecutable;
            PathToInput = pathToInput;
        }

        public void RunSumbission()
        {
            for (int i = 1; i <= 7; i ++)
            {
                string[] currentInput = Directory.GetFiles(PathToInput, i.ToString(), SearchOption.AllDirectories);
                string[] currentOutput = Directory.GetFiles(PathToOutput, i.ToString(), SearchOption.AllDirectories);

                if (currentInput.Length != 1)
                {
                    Result = SubmissionResult.FailedToLocateInput;
                    return; 
                }

                if (currentOutput.Length != 1)
                {
                    Result = SubmissionResult.FailedToLocateOutput;
                    return;
                }

                var testCase = new TestCase
                {
                    Id = i + 1,
                    PathToExecutable = PathToExecutable,
                    PathToInputFile = currentInput[0],
                    PathToExpectedOutputFile = currentOutput[0]
                };

                TestCaseVerdict verdict = testCase.InitTestCase();

                if (verdict != TestCaseVerdict.Passed)
                    break;
            }
        }
    }
}
