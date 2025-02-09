
using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Win32;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Data;

namespace ChallengeTiles.Server
{
    public class Program
    {
        //App initialization
        public static void Main(string[] args)
        {
            //1. configure server
            var builder = WebApplication.CreateBuilder(args);

            //2. Configure configuration to read environment-specific settings
            //Load configuration based on the environment (Development or Production)
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            //3. configure db connection
            /*read db type from constant, referenceing in appsettings.json or environment variables
              IConfiguration in ASP.NET Core accesses config values
              allows switching of db type and configure based on environment (locally, ec2, lambda)*/
            var dbType = builder.Configuration.GetValue<string>(Constants.DbType);

            //use Helpers rather than hard coding db info. can use either MySQL db or Mongo db
            if (dbType == "MySQL")
            {
                builder.Services.AddDbContext<ITilesDbContext, MysqlDbContext>(
                    options => MysqlDbContext.Configure(ConnectionHelper.GetMySqlConnectionString())(options)
                );
            }
            else if (dbType == "MongoDB")
            {
                var mongoSettings = new MongoDbSettings
                {
                    ConnectionString = ConnectionHelper.GetMongoConnectionString(),
                    DatabaseName = ConnectionHelper.GetMongoDbName()
                };

                //only one instance of MongoDbContext created, then reused
                builder.Services.AddSingleton(mongoSettings);
                builder.Services.AddSingleton<MongoDbContext>();
            }
            else
            {
                throw new InvalidOperationException("Unsupported database type.");
            }

            //4. configure CORS policy
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins(allowedOrigins) // Load allowed origins from config
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });


            //5. Add services to the container.
            builder.Services.AddControllers(); //enable MVC controllers 
            // Swagger services (added from asp.net core project)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); //register swagger into apps dependency injection container

            //6. Build application
            var app = builder.Build();

            //7. middleware
            app.UseCors("AllowedFrontend");

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers(); //maps routes

            //8. Enable swagger in dev
            if (app.Environment.IsDevelopment()) //Swagger only enabled in Development envoronment
            {
                app.UseSwagger();   //generate Swagger JSON file that describes API
                app.UseSwaggerUI(); //alows testing of API directly in browser
            }

            //9. start app
            app.Run();
        }
    }
}
