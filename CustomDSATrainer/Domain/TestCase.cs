using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain
{
    public class TestCase
    {
        public int Id { get; set; }
        public required string PathToInputFile { get; set; }
        public required string PathToOutputFile {  get; set; }
        public required string PathToExpectedOutputFile { get; set; }

        public required decimal TimeLimit { get; set; } = 1.0M; // in seconds
        public required uint MemoryLimit { get; set; } = 64; // in MB
        public TestCaseVerdict Verdict { get; set; } = TestCaseVerdict.NoVerdict;

        public long TimeOfStarting { get; set; } // Unix timestamp
        public decimal ExecutionTime { get; set; }

        public int ProblemId { get; set; }
        public Problem Problem { get; set; }

        public TestCase() { }
        public TestCase(string pathToInputFile, string pathToOutputFile, string pathToExpectedOutputFile, decimal timeLimit, uint memoryLimit)
        {
            PathToInputFile = pathToInputFile;
            PathToOutputFile = pathToOutputFile;
            PathToExpectedOutputFile = pathToExpectedOutputFile;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
        }

    }
}
