namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IProblemRepository
    {
        Problem? GetFromId(int id);
        void SaveToDatabase(Problem problem);
        Tuple<string, string> GetUnsolvedData();
        Problem? GetRevision();
        Problem? GetRevisionWithCategories(string categories);
    }
}
