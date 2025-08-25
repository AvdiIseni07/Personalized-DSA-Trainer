namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface ITestCaseRepository
    {
        void SaveToDatabase(TestCase testCase);
    }
}
