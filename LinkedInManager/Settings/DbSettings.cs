using Microsoft.Data.SqlClient;

namespace LinkedInManager.Settings
{
    public class DbSettings
    {
        public string Server { get; set; } = string.Empty;

        public string Database { get; set; } = string.Empty;

        public bool IntegratedSecurity { get; set; }

        public string User { get; set; } = string.Empty;

        public string Pass { get; set; } = string.Empty;

        public string GetSqlConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = Server;
            builder.InitialCatalog = Database;
            builder.IntegratedSecurity = IntegratedSecurity;
            builder.UserID = User;
            builder.Password = Pass;
            builder.TrustServerCertificate = true;

            return builder.ToString();
        }
    }
}
