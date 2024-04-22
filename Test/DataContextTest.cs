using LinkedInManager.Data;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Test
{
    public class Tests
    {
        public AppSettings? AppSettings { get; set; }
        [SetUp]
        public void Setup()
        {
            AppSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true)
            .Build()
            .Get<AppSettings>();  
        }

        [Test]
        public void Recreate()
        {
            var dbContext = DataContext.NewDataContext(AppSettings!.DbSettings.GetSqlConnectionString());

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            dbContext.AppUsers.Add(new LinkedInManager.Entities.AppUser()
            { Email = "tuf.bit85@gmail.com", FirstName = "Tuf", LastName = "Bit", SMTPAppPassword = "zaof dwaw nikw vnne" });   
        }

        [Test]
        public void Migrate()
        {
            var dbContext = DataContext.NewDataContext(AppSettings!.DbSettings.GetSqlConnectionString());

            dbContext.Database.Migrate();
        }
    }
}