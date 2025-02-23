using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public class GameRepository
    {
        //class handles CRUD operations for Game entities
        private readonly MysqlDbContext _dbContext;

        public void CreateGame(Game game, int numberOfColors, int numberOfTiles, List<Player> players)
        {
            //get values for number of tiles and colors set in game
            game.NumberOfColors = numberOfColors;
            game.NumberOfTiles = numberOfTiles;

            _dbContext.Game.Add(game); //INSERT game record in db

            foreach (var player in players)
            {
                //create hand for each Player and associate it with current Game
                var playerHand = new Hand
                {
                    GameId = game.GameId,
                    PlayerId = player.PlayerId
                };

                _dbContext.Hand.Add(playerHand); //INSERT hand record into db
            }

            _dbContext.SaveChanges();
        }

        //add a new game record
        public void AddGame(Game game)
        {
            _dbContext.Game.Add(game);
            _dbContext.SaveChanges();
        }

        //retrieve a game by id
        public Game GetGameById(int gameId)
        {
            var game = _dbContext.Game
                .Include(g => g.Hands)  //include the related hands
                .ThenInclude(h => h.Player)  //include the related players
                .FirstOrDefault(g => g.GameId == gameId);

            return game;
        }

        public void UpdateGameHands(Game game)
        {
            _dbContext.Game.Update(game);
            foreach (var hand in game.Hands)
            {
                _dbContext.Hand.Update(hand); //update each player's hand
            }
            _dbContext.SaveChanges();
        }
    }
}
