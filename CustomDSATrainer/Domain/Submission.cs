using CustomDSATrainer.Domain.Enums;

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

        public Submission() { }

        public Submission(string pathToExecutable)
        {
            PathToExecutable = pathToExecutable;
        }
    }
}
