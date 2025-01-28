
using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Win32;

namespace ChallengeTiles.Server
{
    public class Program
    {
        //App initialization
        public static void Main(string[] args)
        {
            //configure server
            var builder = WebApplication.CreateBuilder(args);

            //use Connection Helper rather than hard coding db info
            builder.Services.AddDbContext<Data.TilesDbContext>(options =>
                options.UseMySql(ConnectionHelper.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 25))));

            // Add services to the container.
            builder.Services.AddControllers(); //enable MVC controllers
            
            // Swagger (added from asp.net core project)
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
