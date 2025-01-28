using ChallengeTiles.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public interface TilesDbContext
    {
        DbSet<Player> Players { get; }
        DbSet<Game> Games { get; }
        DbSet<Tile> Tiles { get; }
    }
}
