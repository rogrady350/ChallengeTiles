using Amazon.RDS.Util;

public class ConnectionHelper
{
    //connection string for using MySQL
    public static string GetMySqlConnectionString()
    {
        var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
        var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "Challenge_Tiles";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        var useIAMAuth = Environment.GetEnvironmentVariable("USE_IAM_AUTH") == "true"; //only true in AWS, not used in dev

        //Use for AWS - using IAM. No password used.
        if (useIAMAuth)
        {
            //generate IAM Token for RDS authentication
            string region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";
            return $"Server={server};Database={database};User={user};Password={GenerateRDSAuthToken(server, user, region)};SSL Mode=Required;";
        }
        //otherwise use password
        else
        {
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
            return $"Server={server};Database={database};User={user};Password={password};";
        }
    }


    //generates IAM Authentication Token
    private static string GenerateRDSAuthToken(string server, string user, string region)
    {
        // Convert string region to Amazon.RegionEndpoint
        var awsRegion = Amazon.RegionEndpoint.GetBySystemName(region);

        return RDSAuthTokenGenerator.GenerateAuthToken(awsRegion, server, 3306, user);
    }
}
