using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;

namespace ChallengeTiles.Server.Services
{
    //Class purpose: holds game play logic
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly PlayerRepository _playerRepository;

        public GameService(GameRepository gameRepository, PlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
        }

        public Game StartNewGame(List<int> playerIds, int numberOfColors, int numberOfTiles)
        {
            //1. fetch player objects from DB by playerId
            var players = new List<Player>();

            foreach (var playerId in playerIds)
            {
                var player = _playerRepository.GetPlayerById(playerId);
                if (player != null)
                {
                    players.Add(player);
                }
            }

            //2. create a new Game
            Game game = new Game(numberOfColors, numberOfTiles);

            //3. save the Game to the database
            _gameRepository.AddGame(game);

            //4. add Players and deal Hands
            game.AddPlayers(players, numberOfTiles, game.TileDeck);

            //5. deal the tiles (numberOfTiles is Tiles per hand. multiply by number of players playing)
            game.DealTiles(numberOfTiles * playerIds.Count); 

            //6.update database with populated Hands
            _gameRepository.UpdateGameHands(game);

            //7.display message asking for starting player


            return game;
        }

        //Player picks up a Tile from the TileDeck
        public void PlayerPickUpTile(int gameId, int playerId)
        {
            Game? game = _gameRepository.GetGameById(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game {gameId} not found");
            }

            game.PickUpTile(playerId);
        }

        //Status of Tile
        public PlacementStatus PlaceTile(int gameId, int playerId, Tile tile, int x, int y)
        {
            Game? game = _gameRepository.GetGameById(gameId);
            if (game == null)
                throw new InvalidOperationException("Game not found.");

            //get result of placemet attempt
            PlacementStatus result = game.PlaceTileOnBoard(playerId, tile, x, y);

            //print result to console for testing
            switch (result)
            {
                case PlacementStatus.Success:
                    Console.WriteLine("Tile placed successfully!");
                    break;
                case PlacementStatus.PositionOccupied:
                    Console.WriteLine("That position is already occupied.");
                    break;
                case PlacementStatus.NoAdjacentTile:
                    Console.WriteLine("The tile must be placed adjacent to another tile.");
                    break;
                case PlacementStatus.InvalidTile:
                    Console.WriteLine("Tile must match the color or number of an adjacent tile.");
                    break;
            }

            return result;
        }

        public Game GetGameById(int gameId)
        {
            var game = _gameRepository.GetGameById(gameId);
            return game;
        }
    }
}
