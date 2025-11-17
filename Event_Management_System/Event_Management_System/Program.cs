using System;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Data;

namespace Event_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MAS Project EF Core Test ===");

            var options = new DbContextOptionsBuilder<MasDbContext>()
                .UseSqlite("Data Source=mas.db")
                .Options;

            using (var context = new MasDbContext(options))
            {
                // context.Database.EnsureCreated();
                context.Database.Migrate();
                
                DataInitializer.Seed(context);

                Console.WriteLine(" Database initialized.");
            }

            Console.WriteLine("=== Program finished ===");
        }
    }
}