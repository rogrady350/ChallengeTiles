namespace ChallengeTiles.Server.Helpers
{
    public class ConnectionHelper
    {
        //connection string for using MySQL
        public static string GetMySqlConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "Challenge_Tiles"; 
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Database environment variables are not set properly.");
            }

            return $"Server={server};Database={database};User={user};Password={password};";
        }
    }
}
