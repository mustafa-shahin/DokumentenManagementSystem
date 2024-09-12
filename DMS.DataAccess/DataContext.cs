using DMS.Model;
using Microsoft.EntityFrameworkCore;

namespace DMS.DataAccess;

public class DataContext : DbContext
{
    public DbSet<Benutzer> Benutzer { get; set; }
    public DbSet<Dokument> Dokumente { get; set; }
    public DbSet<Ordner> Ordner { get; set; }

    public string Path { get; set; }

    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        Path = System.IO.Path.Join(path, "dms.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={Path}");
}