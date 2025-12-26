using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Event_Management_System.Data;

namespace Event_Management_System.Data
{
    public class MasDbContextFactory : IDesignTimeDbContextFactory<MasDbContext>
    {
        public MasDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MasDbContext>();

            var dbPath = DbPathProvider.GetDbPath();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new MasDbContext(optionsBuilder.Options);
        }
    }
}