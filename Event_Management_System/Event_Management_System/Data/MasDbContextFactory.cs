using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Event_Management_System.Data
{
    public class MasDbContextFactory : IDesignTimeDbContextFactory<MasDbContext>
    {
        public MasDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MasDbContext>();
            optionsBuilder.UseSqlite("Data Source=mas.db");

            return new MasDbContext(optionsBuilder.Options);
        }
    }
}