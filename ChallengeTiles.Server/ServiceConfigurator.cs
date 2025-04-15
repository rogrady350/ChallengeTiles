using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Services;

namespace ChallengeTiles.Server
{
    public static class ServiceConfigurator
    {
        //IServiceCollection - Part of ASP.NET Core's DI. Services need at runtime
        public static void ConfigureAppServices(IServiceCollection services)
        {
            //2. configure CORS policy
            /*read allowed origins from env variable
             ALLOWED_ORIGINS set in AWS Lambda to switch allowed frontend URLs*/
            var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")
                               ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                               ?? new[] { "https://localhost:63304" };

            services.AddCors(options =>
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

            //3.Configure DB connection
            services.AddDbContext<ITilesDbContext, MysqlDbContext>(options =>
                options.UseMySql(
                    /*used to get connection string from ConnectionHelper
                     ensures correct configuration used based on environment*/
                    ConnectionHelper.GetMySqlConnectionString(),
                    new MySqlServerVersion(new Version(8, 0, 25))
                )
            );


            //4. add services to the container
            //4.1 store active games in memopry
            services.AddSingleton<GameStateManager>();

            //4.2 add repositories
            services.AddScoped<GameRepository>();
            services.AddScoped<PlayerRepository>();

            //4.3 add services
            services.AddScoped<GameService>();
            services.AddScoped<PlayerService>();

            //4.4 add controllers
            services.AddControllers(); //enable MVC controllers. Registers ALL controllers, dont need to individually add
        }
    }
}
