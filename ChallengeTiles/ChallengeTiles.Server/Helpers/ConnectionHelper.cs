namespace ChallengeTiles.Server.Helpers
{
    public class ConnectionHelper
    {
        //connection string for using MySQL
        public static string GetMySqlConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "challengetiles-mysql.cp6syg6cu1dj.us-east-2.rds.amazonaws.com"; //AWS RDS endpoint
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "Challenge_Tiles"; //Need to create db in MySQL workbench
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "admin"; //RDS user name
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "challengetiles"; //RDS pw

            return $"Server={server};Database={database};User={user};Password={password};";
        }

        //connection string for using MongoDB
        public static string GetMongoConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("MONGO_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017"; // Default MongoDB port
            var database = Environment.GetEnvironmentVariable("MONGO_DB") ?? "WildTiles";
            var user = Environment.GetEnvironmentVariable("MONGO_USER");
            var password = Environment.GetEnvironmentVariable("MONGO_PASSWORD");

            //with authentication
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                var encodedUser = Uri.EscapeDataString(user);
                var encodedPassword = Uri.EscapeDataString(password);
                return $"mongodb://{encodedUser}:{encodedPassword}@{host}:{port}/{database}?authSource=admin";
            }
            //no authentication
            else
            {                
                return $"mongodb://{host}:{port}/{database}";
            }
        }

        //dynamically get name of db. MongoDB connections do not specify a database in the connection string
        public static string GetMongoDbName()
        {
            return Environment.GetEnvironmentVariable("MONGO_DB") ?? "WildTiles";
        }
    }
}
