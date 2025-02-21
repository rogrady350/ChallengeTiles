using Microsoft.EntityFrameworkCore;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GamePlay;

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
        public DbSet<Player> Player { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Hand> Hand { get; set; }

        //OnModelCreating: EF Core uses for configurations
        /*Configures entity relationships, constraints, indexes, table management
         Use even if DB manually built in MySQL workbench
         Explicity define constraints to ensure EF Core does use defaults
         Allows future migrations if needed*/

    }
}
