using CustomDSATrainer.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CustomDSATrainer.Persistance
{
    public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public ProjectDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();

            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            return new ProjectDbContext(optionsBuilder.Options);
        }
            
    }
}
