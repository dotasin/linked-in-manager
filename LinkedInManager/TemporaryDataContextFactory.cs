using LinkedInManager.Data;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LinkedInManager
{
    /// <summary>
    /// This class is used by migration to inspect the assembly (this dll)
    /// in order to find what database to use (basically we are passing
    /// connection string for migration tool here).
    /// </summary>
    public class TemporaryDataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var settings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .Build()
                .Get<AppSettings>();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(settings!.DbSettings.GetSqlConnectionString())
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                .Options;

            // Note: we pass empty user id, as we shouldn't use this context
            // for anything for except schema creation (migration tool)
            return new DataContext(options);
        }
    }
}