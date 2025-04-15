
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

            //debug: checks if using IAM
            var useIAMAuth = Environment.GetEnvironmentVariable("USE_IAM_AUTH") == "true";
            //shows where running
            if (useIAMAuth)
            {
                Console.WriteLine("Running in AWS - Using IAM Authentication for RDS.");
            }
            else
            {
                Console.WriteLine("Running Locally - Using username/password authentication.");
            }

            //2. configure CORS policy, 3. configure db connection, 4, 4.1-4.4. add services to the container
            //Centalize builder services config in ServicConfigurator.cs to use in both lambda and local/ebs. Avoid same code in 2 places
            ServiceConfigurator.ConfigureAppServices(builder.Services);

            //3.5 Swagger services (added from asp.net core project for testing)
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