using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DMS.DataAccess;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        // Dynamischer Pfad für die SQLite-Datenbank
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = System.IO.Path.Join(Environment.GetFolderPath(folder), "dms.db");

        // SQLite-Datenbank mit dynamischem Pfad konfigurieren
        optionsBuilder.UseSqlite($"Data Source={path}");

        return new DataContext(optionsBuilder.Options);
    }
}