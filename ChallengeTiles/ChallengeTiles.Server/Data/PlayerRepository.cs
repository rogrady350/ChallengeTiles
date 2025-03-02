using ChallengeTiles.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public class PlayerRepository
    {
        //class handles CRUD operations for Player entities
        private readonly MysqlDbContext _dbContext;

        //constructor
        public PlayerRepository(MysqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //retrieve player by id
        public Player GetPlayerById(int playerId)
        {
            return _dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);
        }

        //retrieve player by name
        public Player GetPlayerByUsername(string username)
        {
            return _dbContext.Player.FirstOrDefault(p => p.Username == username);
        }
    }
}
