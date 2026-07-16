namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface ITestCaseRepository
    {
        Task SaveToDatabase(TestCase testCase);
    }
}
