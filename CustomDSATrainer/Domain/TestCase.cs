using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain
{
    public class TestCase
    {
        public int Id { get; set; }
        public string PathToExecutable { get; set; }
        public string Input { get; set; } = string.Empty;
        public string ExpectedOutput { get; set; } = string.Empty;
        public long TimeLimit { get; set; } = 1000; // in milliseconds
        public uint MemoryLimit { get; set; } = 64; // in MB
        public TestCaseVerdict Verdict { get; set; } = TestCaseVerdict.NoVerdict;
        public decimal ExecutionTime { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public TestCase() { }
        public TestCase(string input, uint timeLimit, uint memoryLimit)
        {
            Input = input;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
        }        
    }
}
