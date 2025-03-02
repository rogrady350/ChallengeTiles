
using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Win32;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Data;
using DotNetEnv;
using ChallengeTiles.Server.Services;

namespace ChallengeTiles.Server
{
    public class Program
    {
        //App initialization
        public static void Main(string[] args)
        {
            //1. configure server
            Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            //2.1. configure db connection
            /*adds DbContext to dependency injection container.
             tells app how to configure MysqlDbContext and provides necessary connection string for MySQL*/
            builder.Services.AddDbContext<ITilesDbContext, MysqlDbContext>(options =>
                options.UseMySql(
                    /*used to get connection string from ConnectionHelper
                     ensures correct configuration used based on environment*/
                    ConnectionHelper.GetMySqlConnectionString(),
                    new MySqlServerVersion(new Version(8, 0, 25))
                )
            );

            //3. configure CORS policy
            /*read allowed origins from env variable
             ALLOWED_ORIGINS set in AWS Lambda or EC2 to switch allowed frontend URLs*/
            var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")
                               ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                               ?? new[] { "http://localhost:63304" };

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        // Load allowed origins from config
                        policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
                    });
            });


            //4. add services to the container
            //4.1 add repositories
            builder.Services.AddScoped<GameRepository>();
            builder.Services.AddScoped<PlayerRepository>();

            //4.2 add services
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<PlayerService>();

            //4.3 add controllers
            builder.Services.AddControllers(); //enable MVC controllers. Registers ALL controllers, dont need to individually add
            
            //4.4 Swagger services (added from asp.net core project for testing)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); //register swagger into apps dependency injection container

            //4.5 AWS Lambda service
            builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi); //deploy to lambda

            //5. Build application
            var app = builder.Build();

            //6.1. middleware
            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers(); //maps routes

            //6.2. enable swagger in dev
            if (app.Environment.IsDevelopment()) //Swagger only enabled in Development envoronment
            {
                app.UseSwagger();   //generate Swagger JSON file that describes API
                app.UseSwaggerUI(); //alows testing of API directly in browser
            }

            //7. start app
            app.Run();
        }
    }
}