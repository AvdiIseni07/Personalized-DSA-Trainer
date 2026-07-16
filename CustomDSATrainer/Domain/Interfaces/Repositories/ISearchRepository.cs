namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface ISearchRepository
    {
        Task SaveToDatabase(Search search);
    }
}
