using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.DTO;
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

            Console.WriteLine("Fetching players");

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
            Console.WriteLine("Creating game");
            _gameRepository.CreateGame(game, numberOfColors, numberOfTiles, players);

            //4. add Players and Hands to Game object
            Console.WriteLine("Adding players and hands to game");
            game.AddPlayers(players, numberOfTiles, game.TileDeck);

            //5. deal the tiles (numberOfTiles is Tiles per hand. multiply by number of players playing)
            Console.WriteLine("Dealing tiles");
            game.DealTiles(numberOfTiles * playerIds.Count);

            //6.update database with initial populated Hands
            Console.WriteLine("Updating hands with dealt tiles");
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

        //result of place Tile on board
        public ServiceResponse<string> PlayerPlaceTile(int gameId, int playerId, Tile tile, int x, int y)
        {
            var response = new ServiceResponse<string>();

            Game? game = GetGameById(gameId);
            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            //get result of placemet attempt
            PlacementStatus result = game.PlaceTile(playerId, tile, x, y);

            //print result to console for testing
            switch (result)
            {
                case PlacementStatus.Success:
                    response.Success = true;
                   response.Data = "Tile placed successfully!";
                    break;
                case PlacementStatus.PositionOccupied:
                    response.Success = false;
                    response.Message = "That position is already occupied.";
                    break;
                case PlacementStatus.NoAdjacentTile:
                    response.Success = false;
                    response.Message = "The tile must be placed adjacent to another tile.";
                    break;
                case PlacementStatus.InvalidTile:
                    response.Success = false;
                    response.Message = "Tile must match the color or number of an adjacent tile.";
                    break;
                default:
                    response.Success = false;
                    response.Message = "An unknon error occured";
                    break;
            }

            //call turn handling method to switch turns or end game
            game.NextTurn();

            //check for end of game
            if (game.GameOver == true)
            {
                _gameRepository.FinalizeGame(game, game.Score);
                response.Message += "Game comple. Congratulations!";
            }

            return response;
        }

        //method to get the game Id from repository to send to GameController. also can be used in functions rather than calling repository directly
        public Game GetGameById(int gameId)
        {
            Game game = _gameRepository.GetGameById(gameId); //retrieve game by id
            return game;
        }

        //get all games in db (not needed at this time, available for future use)
        public IEnumerable<Game> GetAllGames()
        {
            IEnumerable<Game> gameList = _gameRepository.GetAllGames();
            return gameList;
        }

        //method to retrieve relevent game info for viewing on stats page
        public GameDTO GetGameDetails(int gameId)
        {
            //get game info from db (includes associated players and hands)
            var game = _gameRepository.GetGameById(gameId);
       
            //game not found
            if (game == null)
            {
                return null;
            }

            //map game data
            GameDTO gameInfo = new GameDTO
            {
                GameId = game.GameId,
                //map players associated with game
                Players = game.Hands.Select(h => new PlayerDTO
                {
                    PlayerId = h.PlayerId,
                    Name = h.Player.Name,
                    //map hands associated with players
                    Hands = new List<HandDTO>
                    {
                        //map hand info
                        new HandDTO
                        {
                            HandId = h.HandId,
                            Tiles = h.Tiles
                        }
                    }
                }).ToList()
            };

            return gameInfo;
        }

        //retrieve current states of Game object for frontend
        public GameStateDTO GetGameState(int gameId)
        {
            Game game = GetGameById(gameId);
            if (game == null)
            {
                return null;
            }

            GameStateDTO gameState = new GameStateDTO
            {
                GameId = game.GameId,
                Players = game.Hands.Select(h => new PlayerDTO
                {
                    PlayerId = h.PlayerId,
                    Name = h.Player.Name
                }).ToList(),
                Hands = game.Hands.Select(h => new GameHandDTO
                {
                    HandId = h.HandId,
                    HandTiles = h.HandTiles?.Select(t => new TileDTO
                    {
                        TileId = t.Id,
                        Number = t.Number,
                        Color = t.Color,
                        ImageUrl = t.TileImageUrl
                    }).ToList() ?? new List<TileDTO>() //handle empty list before deal
                }).ToList(),
                TileDeck = game.TileDeck.Tiles.Select(t => new TileDTO
                {
                    TileId = t.Id,
                    Number = t.Number,
                    Color = t.Color,
                    ImageUrl = t.TileImageUrl
                }).ToList(),
                GameBoard = game.GameBoard.PlacedTiles.Select(p => new TilePlacementDTO
                {
                    Tile = new TileDTO
                    {
                        TileId = p.Tile.Id,
                        Number = p.Tile.Number,
                        Color = p.Tile.Color,
                        ImageUrl = p.Tile.TileImageUrl
                    }
                }).ToList(),
                CurrentScore = game.Score
            };

            return gameState;
        }
    }
}