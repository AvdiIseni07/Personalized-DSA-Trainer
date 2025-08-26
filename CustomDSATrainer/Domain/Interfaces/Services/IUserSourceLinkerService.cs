using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IUserSourceLinkerService
    {
        Task<TestCaseVerdict> RunCppExecutable(TestCase testCase);
    }
}
