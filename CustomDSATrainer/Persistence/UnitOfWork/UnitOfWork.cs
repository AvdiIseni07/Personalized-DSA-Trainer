using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.UnitOfWork;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace CustomDSATrainer.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDbContext _context;
        private IDbContextTransaction? _transaction;

        public IActivityLogRepository ActivityLogRepository { get; }
        public IAIReviewRepository AIReviewRepository { get; }
        public IProblemRepository ProblemRepository { get; }
        public ISubmissionRepository SubmissionRepository { get; }
        public ITestCaseRepository TestCaseRepository { get; }
        public IUserProgressRepository UserProgressRepository { get; }

        public UnitOfWork(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
            ActivityLogRepository = new ActivityLogRepository(_context);
            AIReviewRepository = new AIReviewRepository(_context);
            ProblemRepository = new ProblemRepository(_context);
            SubmissionRepository = new SubmissionRepository(_context);
            TestCaseRepository = new TestCaseRepository(_context);
            UserProgressRepository = new UserProgressRepository(_context);
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _context.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
