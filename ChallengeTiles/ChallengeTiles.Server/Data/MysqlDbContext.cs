using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Models;

namespace ChallengeTiles.Server.Data
{
    public class MysqlDbContext : DbContext, TilesDbContext
    {
        //constructor, creates connection to db, use dependency injection.
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Tile> Tiles { get; set; }

        public static Action<DbContextOptionsBuilder> Configure(string connectionString)
        {
            return options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25)));
        }
    }
}
