using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.DTO;
using ChallengeTiles.Server.Models.GameLogic;
using System;

namespace ChallengeTiles.Server.Services
{
    //Class purpose: holds game play logic
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly GameStateManager _gameStateManager; //singleton for active games
        public GameService(GameRepository gameRepository, PlayerRepository playerRepository, GameStateManager gameStateManager)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _gameStateManager = gameStateManager;
        }

        public Game StartNewGame(List<int> playerIds, int numberOfColors, int numberOfTiles)
        {
            //1. fetch Players from DB by playerId (selected on front end)
            var players = new List<Player>(); //list of Player Ojbects

            Console.WriteLine("GameService StartNewGame debug 1 - Fetching players");

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
            Console.WriteLine("GameService StartNewGame debug 2- Creating game object");
            Game game = new Game(numberOfColors, numberOfTiles);

            //3. create both Game and Hand records in db (CreateGame method in GameRepository handles both)
            Console.WriteLine("GameService StartNewGame debug 3- Creating game db records");
            _gameRepository.CreateGame(game, numberOfColors, numberOfTiles, players);

            //4. add Players and Hands to Game objects
            Console.WriteLine("GameService StartNewGame debug 4 - Adding players and hands to game");
            game.AddPlayers(players, numberOfTiles, game.TileDeck);

            //5. deal the tiles
            //numberOfTiles is Tiles per hand. moved multiplication calculation inside Game.cs. only send numberOfTiles now)
            Console.WriteLine("GameService StartNewGame debug 5 - Dealing tiles");
            game.DealTiles(numberOfTiles);

            //6. update database with initial populated Hands
            Console.WriteLine("GameService StartNewGame debug 6 - Updating db hands with dealt tiles");
            _gameRepository.UpdateGameHands(game);

            //7. store active game in memory
            Console.WriteLine($"GameService StartNewGame debug 7 - Storing game ID {game.GameId} in active games.");
            _gameStateManager.StoreGame(game.GameId, game);

            return game;
        }

        //set starting player to instantiate current player
        public void GameSetStartingPlayer(int gameId, int playerId)
        {
            Game? game = _gameStateManager.GetGame(gameId); //in memory gameId
            if (game == null)
            {
                throw new InvalidOperationException("Game not found");
            }

            game.SetStartingPlayer(playerId);
        }

        //Player picks up a Tile from the TileDeck
        public void PlayerPickUpTile(int gameId, int playerId)
        {
            Game? game = _gameStateManager.GetGame(gameId); //in memory gameId
            if (game == null)
            {
                throw new InvalidOperationException($"Game {gameId} not found");
            }

            game.PickUpTile(playerId);
        }

        //result of place Tile on board
        public ServiceResponse<string> PlayerPlaceTile(int gameId, int playerId, Tile tile, int x, int y)
        {
            Console.WriteLine($"GameService PlayerPlace tile debug - Processing tile placement: " +
                $"Game ID: {gameId}, Player ID: {playerId}, Tile ID: {tile?.Id}, Color: {tile?.Color}, X: {x}, Y: {y}");

            var response = new ServiceResponse<string>();

            Game? game = _gameStateManager.GetGame(gameId); //in memory gameId
            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                Console.WriteLine($"Game {gameId} not found in memory.");
                return response;
            }
            else
            {
                //debug: show what backend is sending to see if any data is missing
                Console.WriteLine($"GameService debug - Game State - Returning state: " +
                    $"{game.GameId}, Players: {game.PlayerHands.Count}, Hands: {game.Hands.Count}, GameOver: {game.GameOver}");
            }
            //get result of placemet attempt
            PlacementStatus result = game.PlaceTile(playerId, tile, x, y);

            switch (result)
            {
                case PlacementStatus.Success:
                    //call turn handling method to switch turns or end game
                    game.NextTurn();
                    Console.WriteLine($"GameService debug - Next turn. New current player: {game.CurrentPlayerId}");

                    //check for end of game
                    if (game.GameOver == true)
                    {
                        _gameRepository.FinalizeGame(game, game.Score);
                        response.Message += "Game comple. Congratulations!";
                    }

                    response.Success = true;
                    response.Data = "Tile placed successfully!";
                    break;
                case PlacementStatus.NoHand:
                    response.Success = false;
                    response.Message = "No hand found for player.";
                    break;
                case PlacementStatus.TileNotFound:
                    response.Success = false;
                    response.Message = "Tile not found in player's hand.";
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

            return response;
        }

        //method to get the game Id from repository to send to GameController. also can be used in functions rather than calling repository directly
        public Game GetGameById(int gameId)
        {
            Game game = _gameRepository.GetGameById(gameId); //retrieve game by id from database
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
            Game game = GetGameById(gameId); //db game id
       
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
            var game = _gameStateManager.GetGame(gameId); //in memory game Id
            if (game == null)
            {
                Console.WriteLine($"GameService GameState debug - Game ID {gameId} not found in active games.");
                return null; //return null if the game isn't in memory (gameplay should only be tracking active games)
            }
            Console.WriteLine($"GameService GameState debug - total hands: {game.PlayerHands.Count}");
            Console.WriteLine($"GameService GameState debug - current player ID: {game.CurrentPlayerId}");

            GameStateDTO gameState = new GameStateDTO
            {
                GameId = game.GameId,
                //store Players as a list of objects (only get id and name)
                Players = game.PlayerHands.Values.Select(ph => new PlayerInfoDTO
                {
                    PlayerId = ph.PlayerId,
                    Name = ph.Player.Name
                }).ToList(),
                Hands = game.PlayerHands.Select(ph => new HandGameDTO
                {
                    HandId = ph.Value.HandId, //get HandId from dictionary, not db
                    HandTiles = ph.Value.HandTiles.Select(t => new TileDTO //convert each Tile to JSON
                    {
                        TileId = t.Id,
                        Number = t.Number,
                        Color = t.Color,
                        TileImageUrl = t.TileImageUrl
                    }).ToList()
                }).ToList(),
                TileDeck = game.TileDeck.Tiles.Select(t => new TileDTO
                {
                    TileId = t.Id,
                    Number = t.Number,
                    Color = t.Color,
                    TileImageUrl = t.TileImageUrl
                }).ToList(),
                GameBoard = game.GameBoard.PlacedTiles.Select(pt => new TilePlacementDTO
                {
                    X = pt.X,
                    Y = pt.Y,
                    Tile = new TileDTO
                    {
                        TileId = pt.Tile.Id,
                        Number = pt.Tile.Number,
                        Color = pt.Tile.Color,
                        TileImageUrl = pt.Tile.TileImageUrl
                    }
                }).ToList(),
                CurrentScore = game.Score,
                CurrentPlayerId = game.CurrentPlayerId,
                TileDeckImageUrl = $"{Constants.BucketUrl}/logo.png",
                GameOver = game.GameOver
            };

            Console.WriteLine($"GameService GameState debug - Returning {gameState.GameBoard.Count} tiles on the board.");
            return gameState;
        }
    }
}