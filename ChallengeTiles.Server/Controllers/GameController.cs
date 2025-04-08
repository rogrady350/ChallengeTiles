using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.DTO;
using ChallengeTiles.Server.Models.GameLogic;
using ChallengeTiles.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChallengeTiles.Server.Controllers
{
    //Class purpose: Manage HTTP API end points for game actions

    //define routing
    //maps to game (ASP.NET removes "Controller" from class name to define route)
    [Route("api/[controller]")] //base URL route for controller.
    [ApiController] //Marks class as an API controller

    //Handle route-specific logic
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService; //service handling game logic

        //Constructor creates game instance
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        //POST start a new game (Server side for GameSetupService.js startNewGame)
        [HttpPost("start-game")]
        public IActionResult StartNewGame([FromBody] StartGameRequest request)
        {
            Game game = _gameService.StartNewGame(request.PlayerIds, request.NumberOfColors, request.NumberOfTiles);

            /*return response with game data (GameId, relevent Player data)
             CreatedAtAction returns to client
                a 201 Create HTTP response,
                a Location header with URL to newly created games*/
            return CreatedAtAction(nameof(GetGameById), new { id = game.GameId }, new
            {
                gameId = game.GameId,
                players = game.GetPlayers().Select(p => new
                {
                    playerId = p.PlayerId,
                    name = p.Name,
                }),
                message = $"Welcome to game {game.GameId}"
            });
        }

        //GET retrieve a Game by its id (called in StartNewGame to send newly created Game databack to client)
        [HttpGet("{id:int}")]
        public IActionResult GetGameById(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                gameId = game.GameId,
                players = game.GetPlayers().Select(p => new
                {
                    playerId = p.PlayerId,
                    name = p.Name,
                }),
                tileDeck = game.TileDeck
            });
        }

        //GET retrieve list of games (not needed at this time. available for future use)
        //service returns IEnumerable, client specifies if it needs a list for indexing in query param
        [HttpGet("games")]
        public ActionResult<IEnumerable<Game>> GetGames([FromQuery] bool asList = false)
        {
            var games = _gameService.GetAllGames();

            //convert to list if requested
            if (asList)
            {
                return Ok(games.ToList());
            }

            return Ok(games);
        }

        //POST set the starting player: Client sends gameId, selected playerId to Sever
        [HttpPost("{gameId}/set-starting-player/{playerId}")]
        public IActionResult SetStartingPlayer(int gameId, int playerId)
        {
            Console.WriteLine($"GameController SetStartingPlayer API called - Game ID: {gameId}, Player ID: {playerId}");

            _gameService.GameSetStartingPlayer(gameId, playerId);

            Console.WriteLine("Starting player set successfully in backend.");
            return Ok(new { message = "Starting player set." });
        }

        //POST place a tile on the board - Client sends tile the player wnts to place on board
        //Return's resulting message based on placement verification
        [HttpPost("{gameId}/player-place-tile/{playerId}")]
        public ActionResult<ServiceResponse<string>> PlayerPlaceTile(int gameId, [FromBody] TilePlacementRequest request)
        {
            //debug:
            Console.WriteLine($"GameController PlayerPlaceTile debug - Received request: " +
                $"gameId={gameId}, playerId={request?.PlayerId}, TileId={request?.Tile?.TileId}, Position={request?.Position}");
            if (request == null)
            {
                Console.WriteLine("Error: Request body is null");
                return BadRequest(new { message = "Request body is missing." });
            }
            if (request.Tile == null)
            {
                Console.WriteLine("Error: Tile object is missing");
                return BadRequest(new { message = "Tile object is missing from request." });
            }
            //debug missing color attribute
            if (string.IsNullOrEmpty(request.Tile.Color))
            {
                Console.WriteLine("Error: Tile color is missing");
                return BadRequest(new { message = "Tile color is required." });
            }

            /*Model validation failing. ASP.NET not deserializing Tile.Color.
              Using DTO rather than Tile object - TileDTO used in TilePlacement request to transfer Tile data from client
              Method is both recieving and sending data
              Convert TileDTO to Tile before passing to GameService
              Tile object used for game logic and backend storage*/
            Tile tile = new Tile(request.Tile);

            //place tile function
            ServiceResponse<string> response = _gameService.PlayerPlaceTile(gameId, request.PlayerId, tile, request.Position);

            if (!response.Success)
            {
                return BadRequest(new { message = response.Message });
            }

            Console.WriteLine("Tile placed successfully.");
            return Ok(response);
        }

        //POST pick up a tile from the TileDeck - Client sends request to pick up Tile from TileDeck
        [HttpPost("{gameId}/pick-up-tile")]
        public IActionResult PlayerPickUpTile(int gameId, [FromBody] PickUpTileRequest request)
        {
            _gameService.PlayerPickUpTile(gameId, request.PlayerId);

            return Ok(new { message = "Tile picked up successfully." });
        }

        //GET - Server sends state of current game objects to client
        [HttpGet("{gameId}/game-state")]
        public ActionResult<object> GetGameState(int gameId)
        {
            GameStateDTO gameState = _gameService.GetGameState(gameId);

            if (gameState == null)
            {
                Console.WriteLine($"GameController debug -Game state not found for Game ID: {gameId}");
                return NotFound(new { message = "Game not found" });
            }

            Console.WriteLine($"Game state retrieved for Game ID: {gameId}");
            return Ok(gameState);
        }
    }
}
