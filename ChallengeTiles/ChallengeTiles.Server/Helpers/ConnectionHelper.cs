namespace ChallengeTiles.Server.Helpers
{
    public class ConnectionHelper
    {
        //connection string for using MySQL
        public static string GetMySqlConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "WildTiles";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

            return $"Server={server};Database={database};User={user};Password={password};";
        }

        //connection string for using Mongo
        

    }
}
