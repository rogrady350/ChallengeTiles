using ChallengeTiles.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public interface ITilesDbContext
    {
        //use Task for an async operation. will not stop app during db queries
        Task<List<Player>> GetPlayersAsync(); //
        Task<List<Game>> GetGamesAsync();
        Task<List<Tile>> GetTilesAsync();
    }
}
