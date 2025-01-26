namespace ChallengeTiles.Server.Helpers
{
    public class ConnectionHelper
    {
        public static string GetConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "WildTiles";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

            return $"Server={server};Database={database};User={user};Password={password};";
        }
    }
}
