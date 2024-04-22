using LinkedInManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkedInManager.Data
{
    public class DataContext : DbContext, IDisposable
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<LinkedInEmployee> LinkedInEmployees { get; set; }
        public DbSet<Search> Searches { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Employer> Employers { get; set; }

        protected internal void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Search>().Property(p => p.SearchState).HasConversion<string>();
        }

        public static DataContext NewDataContext(string connectionString) =>
            new DataContext(GetOptions(connectionString));

        private static DbContextOptions GetOptions(string connectionString)
        {
            var options = new DbContextOptionsBuilder()
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }

        // Your DbSet properties and DbContext configuration...

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Dispose();
            }
        }
    }
}