using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public class GameRepository
    {
        //class handles CRUD operations for Game entities
        private readonly MysqlDbContext _dbContext;

        public void CreateGame(Game game, int numberOfColors, int numberOfTiles)
        {
            game.NumberOfColors = numberOfColors;
            game.NumberOfTiles = numberOfTiles;
            _dbContext.Game.Add(game);
            _dbContext.SaveChanges();

            //after saving the game, create hands for each player
            var player1Hand = new Hand { GameId = game.GameId, PlayerId = 1 };
            var player2Hand = new Hand { GameId = game.GameId, PlayerId = 2 };

            _dbContext.Hand.Add(player1Hand);
            _dbContext.Hand.Add(player2Hand);
            _dbContext.SaveChanges();
        }

        //add a new game record
        public void CreateGame(Game game)
        {
            _dbContext.Game.Add(game);
            _dbContext.SaveChanges();
        }

        public Game GetGameById(int gameId)
        {
            var game = _dbContext.Game
                .Include(g => g.Hands)  // Include the related hands
                .ThenInclude(h => h.Player)  // Include the related players
                .FirstOrDefault(g => g.GameId == gameId);

            return game;
        }
    }
}
