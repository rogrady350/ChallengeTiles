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

                Console.WriteLine("checking for existing hand");
                if (existingHand == null)
                {
                    //create hand for each Player and associate it with current Game
                    Hand playerHand = new Hand
                    {
                        GameId = game.GameId,
                        PlayerId = player.PlayerId
                    };

                    Console.WriteLine("creating new hand");
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

        //retrieve all games (no current situations need all games but will have for possible future needs)
        public IEnumerable<Game> GetAllGames()
        {
            return _dbContext.Games.ToList();
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
            foreach (Hand hand in game.Hands)
            {
                //get hand created at start of game
                Hand existingHand = _dbContext.Hands.FirstOrDefault(h => h.GameId == hand.GameId && h.PlayerId == hand.PlayerId);

                //update hand
                if (existingHand != null)
                {
                    Console.WriteLine($"Updating hand for Player {hand.PlayerId} in Game {hand.GameId}. ExistingHand found: {existingHand != null}");
                    Console.WriteLine($"Before update: Player {existingHand.PlayerId} in Game {existingHand.GameId}, Tiles: {existingHand.Tiles}");
                    existingHand.Tiles = hand.Tiles; //store updated Tiles

                    Console.WriteLine($"After update: Player {existingHand.PlayerId} in Game {existingHand.GameId}, Tiles: {existingHand.Tiles}");
                    _dbContext.Entry(existingHand).Property(h => h.Tiles).IsModified = true; //mark as modified so EF tracks update
                    _dbContext.Hands.Update(existingHand);
                }
                else
                {
                    //this should NOT happen. logging for debugging. Confirmed not hitting else block
                    Console.WriteLine($"WARNING: Attempted to insert a new hand for Player {hand.PlayerId} in Game {hand.GameId}");
                }

            }

            _dbContext.SaveChanges(); //Save all updates before updating game
            _dbContext.Games.Update(game); //Now update the game after hands are saved
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
