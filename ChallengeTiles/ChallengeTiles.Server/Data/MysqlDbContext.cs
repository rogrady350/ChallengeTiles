using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Models;

namespace ChallengeTiles.Server.Data
{
    public class MysqlDbContext : DbContext, ITilesDbContext
    {
        /*constructor, creates connection to db
          Usse dependency injection for automatic db context management.
          EF Core provides Object-Relation Mapping (ORM) for relational DB's*/
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options) { }

        //getters, setters. may need to update based on what needs to be stored in db
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Tile> Tiles { get; set; }

        public static Action<DbContextOptionsBuilder> Configure(string connectionString)
        {
            return options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25)));
        }

        //async datat retrieval methods. provides centralized way to fetch data
        public async Task<List<Player>> GetPlayersAsync() => await Players.ToListAsync();
        public async Task<List<Game>> GetGamesAsync() => await Games.ToListAsync();
        public async Task<List<Tile>> GetTilesAsync() => await Tiles.ToListAsync();
    }
}
