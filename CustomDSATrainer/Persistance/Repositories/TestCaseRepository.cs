using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public TestCaseRepository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null.");
        }
        public async void SaveToDatabase(TestCase testCase)
        {
            using (var context = await _contextFactory.CreateDbContextAsync())
            {
                var existingTestCase = await context.TestCase.FindAsync(testCase.Id);

                if (existingTestCase == null)
                {
                    context.TestCase.Add(testCase);
                }
                else
                {
                    context.Entry(existingTestCase).CurrentValues.SetValues(testCase);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
