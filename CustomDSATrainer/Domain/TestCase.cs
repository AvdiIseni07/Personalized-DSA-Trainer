using CustomDSATrainer.Application;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Shared;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain
{
    public class TestCase
    {
        public int Id { get; set; }
        public string PathToExecutable { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }  
        public long TimeLimit { get; set; } = 1000; // in milliseconds
        public uint MemoryLimit { get; set; } = 64; // in MB
        public TestCaseVerdict Verdict { get; set; } = TestCaseVerdict.NoVerdict;
        public decimal ExecutionTime { get; set; }
        private UserSourceLinker _UserSourceLinker { get; set; } 
        private TestCaseRunner _TestCaseRunner { get; set; } = new TestCaseRunner();
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public TestCase() { _UserSourceLinker = new UserSourceLinker(this); }
        public TestCase(string input, uint timeLimit, uint memoryLimit)
        {
            Input = input;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            _UserSourceLinker = new UserSourceLinker(this);
        }
        
        public void SaveToDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var existingTestCase = context.TestCase.Find(this.Id);

                if (existingTestCase == null)
                {
                    context.TestCase.Add(this);
                }
                else
                {
                    context.Entry(existingTestCase).CurrentValues.SetValues(this);
                }

                context.SaveChanges();
            }
        }
        public TestCaseVerdict InitTestCase()
        {
            Verdict = _UserSourceLinker.RunCppExecutable();

            if (Verdict == TestCaseVerdict.Passed)
                Verdict = _TestCaseRunner.RunTest(ExpectedOutput);

            return Verdict;
        }
    }
}
