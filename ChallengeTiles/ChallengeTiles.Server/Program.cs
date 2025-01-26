
using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChallengeTiles.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //App initialization
            //configure server
            var builder = WebApplication.CreateBuilder(args);

            //use Connection Helper rather than hard coding db info
            builder.Services.AddDbContext<Data.TilesDbContext>(options =>
                options.UseMySql(ConnectionHelper.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 25))));

            // Add services to the container.
            builder.Services.AddControllers(); //enable MVC controllers
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Launch server
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            //start app
            app.Run();
        }
    }
}
