using CustomDSATrainer.Domain.Enums;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IProblemRepository
    {
        Task<Problem?> GetFromId(int id);
        void SaveToDatabase(Problem problem);
        Task<Tuple<string, string>> GetUnsolvedData();
        Task<Problem?> GetRevision();
        Task<Problem?> GetRevisionWithCategories(string categories);
        Task<PaginatedList<Problem>> GetPages(string? searchString, string? categories, string? difficulty, ProblemStatus? status, 
                                              int? pageNumber, int? pageSize, DateTime? dateLowerBound, DateTime? dateUpperBound, SortOption sortOption);
    }
}
