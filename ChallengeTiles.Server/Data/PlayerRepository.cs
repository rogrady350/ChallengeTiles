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

        //create a new profile (new record in Player table)
        public void CreateProfile(Player player)
        {
            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
        }

        //retrieve player by id
        public Player GetPlayerById(int playerId)
        {
            return _dbContext.Players.FirstOrDefault(p => p.PlayerId == playerId);
        }

        //retrieve player by name
        public Player GetPlayerByUsername(string username)
        {
            return _dbContext.Players.FirstOrDefault(p => p.Username == username);
        }

        //retrieve player by Name
        public Player GetPlayerByName(string name)
        {
            return _dbContext.Players.FirstOrDefault(p => p.Name == name);
        }

        //retrieve all players.EF gathers all players from db
        //must convert from IEnumerable to a List in Controller if front end needs indexing
        public IEnumerable<Player> GetAllPlayers()
        {
            return _dbContext.Players.ToList();
        }

        //update player profile (only updates modified fields)
        //(future use - no option to edit and update profile in initial release)
        public void UpdatePlayer(Player updatedPlayer, int playerId)
        {
            Player player = _dbContext.Players.Find(playerId);
            //make sure player exists before trying update
            if (player == null)
            {
                throw new KeyNotFoundException("Player not found");
            }

            //fields to update. will only update if user has entered changes.
            if (!string.IsNullOrEmpty(updatedPlayer.Name)) player.Name = updatedPlayer.Name;
            if (!string.IsNullOrEmpty(updatedPlayer.Username)) player.Username = updatedPlayer.Username;
            if (!string.IsNullOrEmpty(updatedPlayer.Name)) player.Password = updatedPlayer.Password;
            if (!string.IsNullOrEmpty(updatedPlayer.Email)) player.Email = updatedPlayer.Email;

            _dbContext.SaveChanges();
        }

        //delete player from database (DbContext will also delete associted Hands)
        //(future use - no option to delete profile in initial release)
        public void DeletePlayer(int playerId)
        {
            Player player = _dbContext.Players.Find(playerId);

            if (player != null)
            {
                _dbContext.Remove(player);
                _dbContext.SaveChanges();
            }
        }
    }
}
