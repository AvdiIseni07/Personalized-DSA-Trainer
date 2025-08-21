namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IProblemRepository
    {
        Task<Problem?> GetFromId(int id);
        void SaveToDatabase(Problem problem);
        Task<Tuple<string, string>> GetUnsolvedData();
        Task<Problem?> GetRevision();
        Task<Problem?> GetRevisionWithCategories(string categories);
    }
}
