using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    /// <summary>
    /// Repository for <see cref="TestCase"/>
    /// </summary>
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public TestCaseRepository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null.");
        }

        /// <summary>
        /// Saves <see cref="TestCase"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="testCase">The <see cref="TestCase"/> that needs to be saved.</param>
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
