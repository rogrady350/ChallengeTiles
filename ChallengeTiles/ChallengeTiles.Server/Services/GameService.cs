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
            _playerRepository = playerRepository;
        }

        public Game StartNewGame(List<int> playerIds, int numberOfColors, int numberOfTiles)
        {
            //1. fetch Players from DB by playerId
            var players = new List<Player>(); //list of Player Ojbects

            foreach (var playerId in playerIds)
            {
                //fetch Players by Id from db to instantiate a list of Player Objects
                var player = _playerRepository.GetPlayerById(playerId);
                if (player != null)
                {
                    players.Add(player);
                }
            }

            //2. create a new Game instance
            Game game = new Game(numberOfColors, numberOfTiles);

            //3. create both Game and Hand records in db (method handles both)
            _gameRepository.CreateGame(game, numberOfColors, numberOfTiles, players);

            //4. add Players and Hands to Game object
            game.AddPlayers(players, numberOfTiles, game.TileDeck);

            //5. deal the tiles (numberOfTiles is Tiles per hand. multiply by number of players playing)
            game.DealTiles(numberOfTiles * playerIds.Count);

            //6.update database with populated Hands
            _gameRepository.UpdateGameHands(game);

            return game;
        }

        //set starting player to instantiate current player
        public void GameSetStartingPlayer(int gameId, int playerId)
        {
            Game game = GetGameById(gameId);
            if (game == null)
            {
                throw new InvalidOperationException("Game not found");
            }

            game.SetStartingPlayer(playerId);
        }

        //Player picks up a Tile from the TileDeck
        public void PlayerPickUpTile(int gameId, int playerId)
        {
            Game? game = GetGameById(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game {gameId} not found");
            }

            game.PickUpTile(playerId);
        }

        //status of Tile
        public PlacementStatus PlayerPlaceTile(int gameId, int playerId, Tile tile, int x, int y)
        {
            Game? game = GetGameById(gameId);
            if (game == null)
                throw new InvalidOperationException("Game not found.");

            //get result of placemet attempt
            PlacementStatus result = game.PlaceTile(playerId, tile, x, y);

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

            //call turn handling method to switch turns or end game
            game.NextTurn();

            //check for end of game
            if (game.GameOver == true)
            {
                _gameRepository.FinalizeGame(game, game.Score);
            }

            return result;
        }

        //method get the game Id from repository to send to GameController. also can be used in functions rather than calling repository directly
        public Game GetGameById(int gameId)
        {
            var game = _gameRepository.GetGameById(gameId);
            return game;
        }
    }
}
