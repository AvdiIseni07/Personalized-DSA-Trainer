using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IUserSourceLinkerService
    {
        TestCaseVerdict RunCppExecutable(TestCase testCase);
    }
}
