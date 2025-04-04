
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

            //1.1. surpress framework debug noise
            builder.Logging.ClearProviders(); //clears all built-in providers
            builder.Logging.AddConsole();     //adds only basic console logging
            builder.Logging.SetMinimumLevel(LogLevel.Warning); //only show warnings/errors

            //2. configure db connection
            /*adds DbContext to dependency injection container.
             tells app how to configure MysqlDbContext and provides necessary connection string for MySQL*/
            //2.1 checks if using IAM
            var useIAMAuth = Environment.GetEnvironmentVariable("USE_IAM_AUTH") == "true";

            //debug - shows where running
            if (useIAMAuth)
            {
                Console.WriteLine("Running in AWS - Using IAM Authentication for RDS.");
            }
            else
            {
                Console.WriteLine("Running Locally - Using username/password authentication.");
            }

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
             ALLOWED_ORIGINS set in AWS Lambda to switch allowed frontend URLs*/
            var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")
                               ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                               ?? new[] { "https://localhost:63304" };

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials(); //cookies, authentication
                    });
            });

            //4. add services to the container
            //4.1 store active games in memopry
            builder.Services.AddSingleton<GameStateManager>();
            
            //4.2 add repositories
            builder.Services.AddScoped<GameRepository>();
            builder.Services.AddScoped<PlayerRepository>();

            //4.3 add services
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<PlayerService>();

            //4.4 add controllers
            builder.Services.AddControllers(); //enable MVC controllers. Registers ALL controllers, dont need to individually add
            
            //4.5 Swagger services (added from asp.net core project for testing)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); //register swagger into apps dependency injection container

            //5. Build application
            var app = builder.Build();

            //6.1. middleware
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers(); //maps routes
            app.UseMiddleware<ExceptionMiddleware>(); //global exception handling

            //6.2 determine port for ebs
            var port = Environment.GetEnvironmentVariable("PORT") ?? "80";

            //6.3. enable swagger in dev and run app with dynamically assigned port
            if (app.Environment.IsDevelopment()) //Swagger only enabled in Development envoronment
            {
                app.UseSwagger();   //generate Swagger JSON file that describes API
                app.UseSwaggerUI(); //alows testing of API directly in browser
                app.UseHttpsRedirection(); //use https redirect locally only

                //start app
                app.Run();
            }
            //assign port for ebs
            else
            {
                app.Run($"http://0.0.0.0:{port}");
            }
        }
    }
}