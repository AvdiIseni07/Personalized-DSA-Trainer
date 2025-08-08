using CustomDSATrainer.Application;
using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain
{
    public class TestCase
    {
        public int Id { get; set; }
        public required string PathToExecutable { get; set; }
        public required string PathToInputFile { get; set; }
        public required string PathToExpectedOutputFile { get; set; }  
        public uint TimeLimit { get; set; } = 1000; // in milliseconds
        public uint MemoryLimit { get; set; } = 64; // in MB
        public TestCaseVerdict Verdict { get; set; } = TestCaseVerdict.NoVerdict;
        public long TimeOfStarting { get; set; } // Unix timestamp
        public decimal ExecutionTime { get; set; }
        private UserSourceLinker _UserSourceLinker { get; set; } = new UserSourceLinker();
        private TestCaseRunner _TestCaseRunner { get; set; } = new TestCaseRunner();
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }

        public TestCase() { }
        public TestCase(string pathToInputFile, uint timeLimit, uint memoryLimit)
        {
            PathToInputFile = pathToInputFile;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
        }
        
        public TestCaseVerdict InitTestCase()
        {
            TimeOfStarting = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();

            _UserSourceLinker.RunCppExecutable(this.PathToExecutable, this.PathToInputFile);
            
            var currentVerdict = _TestCaseRunner.RunTest(PathToExpectedOutputFile);
            if (currentVerdict == TestCaseVerdict.Passed)
            {
                long timeOfEnding = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

                if (TimeOfStarting - timeOfEnding > TimeLimit)
                {
                    currentVerdict = TestCaseVerdict.TimeLimitExceeded;
                }
            }

            return Verdict;
        }
    }
}
