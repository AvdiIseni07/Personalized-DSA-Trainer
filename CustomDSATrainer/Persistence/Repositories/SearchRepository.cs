using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Persistance;

namespace CustomDSATrainer.Persistence.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ProjectDbContext _context;
        public SearchRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task SaveToDatabase(Search search)
        {
            var existingSearch = await _context.Search.FindAsync(search.Id);
            if (existingSearch == null)
            {
                _context.Search.Add(search);
            }
            else
            {
                _context.Search.Entry(existingSearch).CurrentValues.SetValues(search);
            }
        }
    }
}
