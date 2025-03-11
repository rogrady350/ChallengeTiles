using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public interface ITilesDbContext
    {
        /*class defines how app communicates with database
         interface, defines entitites to be implemented
         DbSet - entitites correspond to db tables*/
        DbSet<Player> Players { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<Hand> Hands { get; set; }

        //EF Core method used to save changes to db
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
