using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GamePlay;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public interface ITilesDbContext
    {
        /*class defines how app communicates with database
         interface, defines entitites to be implemented
         DbSet - entitites correspond to db tables*/
        DbSet<Player> Player { get; set; }
        DbSet<Game> Game { get; set; }
        DbSet<Hand> Hand { get; set; }

        //EF Core method used to save changes to db
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
