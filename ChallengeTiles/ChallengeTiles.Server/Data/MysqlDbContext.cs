using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Models;

namespace ChallengeTiles.Server.Data
{
    public class MysqlDbContext : DbContext, ITilesDbContext
    {
        /*constructor, creates connection to db
          Implements ITilesDContext interface
          MySQL relies on EF Core to handle configurations internally
          EF Core provides Object-Relation Mapping (ORM) for relational DB's
          Has built in dependency injection for automatic db context management.*/
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options) { }

        /*Implement interface properties
         EF Core requires properties to be explicitly declared in concrete class for DI*/
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Hand> Hands { get; set; }
    }
}
