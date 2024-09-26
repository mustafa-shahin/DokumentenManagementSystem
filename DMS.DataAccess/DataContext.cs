using DMS.Model;
using Microsoft.EntityFrameworkCore;

namespace DMS.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Benutzer> Benutzer { get; set; }
        public DbSet<Dokument> Dokumente { get; set; }
        public DbSet<Ordner> Ordner { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Ensure this is only configured if not already done
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = System.IO.Path.Join(Environment.GetFolderPath(folder), "dms.db");
                optionsBuilder.UseSqlite($"Data Source={path}");
            }
        }
    }
}
