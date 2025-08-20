using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain
{
    public class Submission
    {
        public int Id { get; set; }
        public required string PathToExecutable { get; set; } = string.Empty;
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
    }
}
