using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        IActivityLogRepository ActivityLogRepository { get; }
        IAIReviewRepository AIReviewRepository { get; }
        IProblemRepository ProblemRepository { get; }
        ISubmissionRepository SubmissionRepository { get; }
        ITestCaseRepository TestCaseRepository { get; }
        IUserProgressRepository UserProgressRepository { get; }

        Task<int> CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
