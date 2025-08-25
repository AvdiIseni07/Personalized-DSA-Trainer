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
        private readonly ProjectDbContext _context;
        public TestCaseRepository(ProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
        }

        /// <summary>
        /// Saves <see cref="TestCase"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="testCase">The <see cref="TestCase"/> that needs to be saved.</param>
        public async void SaveToDatabase(TestCase testCase)
        {
            var existingTestCase = await _context.TestCase.FindAsync(testCase.Id);

            if (existingTestCase == null)
            {
                _context.TestCase.Add(testCase);
            }
            else
            {
                _context.Entry(existingTestCase).CurrentValues.SetValues(testCase);
            }
        }
    }
}
