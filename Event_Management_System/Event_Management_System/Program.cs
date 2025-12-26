using System;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Data;

namespace Event_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MAS Project EF Core Init ===");

            var dbPath = DbPathProvider.GetDbPath();
            Console.WriteLine("DB PATH (Console) => " + dbPath);

            var options = new DbContextOptionsBuilder<MasDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            using (var context = new MasDbContext(options))
            {
                context.Database.Migrate();
                DataInitializer.Seed(context);

                Console.WriteLine("Database initialized successfully.");
            }

            Console.WriteLine("=== Program finished ===");
        }
    }
}


/*using System;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Data;

namespace Event_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MAS Project EF Core Test ===");

            var basePath = AppContext.BaseDirectory;  // bin/Debug/net8.0
            var projectRoot = Path.GetFullPath(Path.Combine(basePath, @"../../../"));
            var dbPath = Path.Combine(projectRoot, "mas.db");
            
            Console.WriteLine("DB PATH (Console) => " + dbPath);
            
            /*
            var options = new DbContextOptionsBuilder<MasDbContext>()
                .UseSqlite("Data Source=mas.db")
                .Options;
                #1#

            using (var context = new MasDbContext())
            {
                // context.Database.EnsureCreated();
                context.Database.Migrate();
                
                DataInitializer.Seed(context);

                Console.WriteLine(" Database initialized.");
            }

            Console.WriteLine("=== Program finished ===");
        }
    }
}*/