
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
            //configure server
            var builder = WebApplication.CreateBuilder(args);

            //read db type from constant, referenceing in appsettings.json
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


            // Add services to the container.
            builder.Services.AddControllers(); //enable MVC controllers 
            // Swagger services (added from asp.net core project)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); //register swagger into apps dependency injection container

            //Launch server
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) //Swagger only enabled in Development envoronment
            {
                app.UseSwagger();   //generate Swagger JSON file that describes API
                app.UseSwaggerUI(); //alows testing of API directly in browser
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers(); //maps routes

            //start app
            app.Run();
        }
    }
}
