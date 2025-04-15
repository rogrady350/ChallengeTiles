using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public class GameRepository
    {
        //class handles CRUD operations for Game entities
        private readonly MysqlDbContext _dbContext;

        //constructor
        public GameRepository(MysqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //new record in game table
        public void CreateGame(Game game, int numberOfColors, int numberOfTiles, List<Player> players)
        {
            //get values for number of tiles and colors set in game
            game.NumberOfColors = numberOfColors;
            game.NumberOfTiles = numberOfTiles;

            _dbContext.Games.Add(game); //INSERT game record in db
            _dbContext.SaveChanges(); //save record to generate a GameId

            //Handle creating Hand record
            foreach (Player player in players)
            {
                //check if the hand already exists before inserting
                var existingHand = _dbContext.Hands
                    .FirstOrDefault(h => h.GameId == game.GameId && h.PlayerId == player.PlayerId);

                Console.WriteLine($"Repository CreateGame debug - checking for existing hand for {player.PlayerId}");
                if (existingHand == null)
                {
                    //create hand for each Player and associate it with current Game
                    Hand playerHand = new Hand
                    {
                        GameId = game.GameId,
                        PlayerId = player.PlayerId
                    };

                    Console.WriteLine("Repository CreateGame debug - creating new hand");
                    _dbContext.Hands.Add(playerHand);
                }
            }

            _dbContext.SaveChanges(); //save changes with updated hands
        }

        //retrieve a game by id
        public Game GetGameById(int gameId)
        {
            var game = _dbContext.Games
                .Include(g => g.Hands)  //include the related hands
                .ThenInclude(h => h.Player)  //include the related players
                .FirstOrDefault(g => g.GameId == gameId);

            return game;
        }

        //retrieve all games (used to gather all games for View Stats page)
        public IEnumerable<Game> GetAllGames()
        {
            var game = _dbContext.Games
                .Include(g => g.Hands)  //include the related hands
                .ThenInclude(h => h.Player)  //include the related players
                .ToList();

            return game;
        }

        //save game updates (no current situations need updating but will have for possible future needs)
        public void UpdateGame(Game game)
        {
            _dbContext.Games.Update(game); //EF updtates db record
            _dbContext.SaveChanges();
        }

        //save hand to db once populated
        public void UpdateGameHands(Game game)
        {
            foreach (var playerHand in game.PlayerHands.Values)
            {
                //get hand created at start of game
                Hand existingHand = _dbContext.Hands.FirstOrDefault(h => h.GameId == playerHand.GameId && h.PlayerId == playerHand.PlayerId);

                Console.WriteLine($"GameRepository UpdateGameHands debug: +" +
                    $" Updating hand {playerHand.HandId} for Player {playerHand.PlayerId} in Game {playerHand.GameId}. ExistingHand found: {existingHand != null}");
                existingHand.Tiles = System.Text.Json.JsonSerializer.Serialize(playerHand.HandTiles); //store updated Tiles
                _dbContext.Entry(existingHand).Property(h => h.Tiles).IsModified = true; //mark as modified so EF tracks update
                _dbContext.Hands.Update(existingHand);
            }

            _dbContext.SaveChanges(); //Commit the game update;
        }

        public void FinalizeGame(Game game, int finalScore)
        {
            game.Score = finalScore;
            _dbContext.Games.Update(game);
            _dbContext.SaveChanges();
        }
    }
}
