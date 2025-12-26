using System;
using System.IO;

namespace Event_Management_System.Data
{
    public static class DbPathProvider
    {
        public static string GetDbPath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var dbDirectory = Path.Combine(appData, "EventManagementSystem");
            Directory.CreateDirectory(dbDirectory);

            return Path.Combine(dbDirectory, "mas.db");
        }
    }
}