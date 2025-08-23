using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface ITestCaseService
    {
        TestCaseVerdict InitTestCase(TestCase testCase);
        Task SaveToDatabase(TestCase testCase);
    }
}
