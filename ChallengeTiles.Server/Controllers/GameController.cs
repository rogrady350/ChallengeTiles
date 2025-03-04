using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.DTO;
using ChallengeTiles.Server.Models.GameLogic;
using ChallengeTiles.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public GameController(GameService gameService, PlayerService playerService)
        {
            _gameService = gameService;
        }

        //POST start a new game
        [HttpPost("start-game")]
        public IActionResult StartNewGame([FromBody] StartGameRequest request)
        {
             try
            {
                //validate numberOfColors - added for testing. swagger not seeing valid values
                if (request.NumberOfColors <= 0 || request.NumberOfColors > Constants.availableColors.Count)
                {
                    return BadRequest(new { message = "Invalid number of colors selected." });
                }

                Game game = _gameService.StartNewGame(request.PlayerIds, request.NumberOfColors, request.NumberOfTiles);

                //return response with game data (GameId, relevent Player data)
                return Ok(new
                {
                    gameId = game.GameId,
                    players = game.GetPlayers().Select(p => new
                    {
                        playerId = p.PlayerId,
                        nameof = p.Name,
                    }),
                    message = "Please select a starting player" //send message to ask user to select starting player.
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //POST set the starting player: Client sends gameId selected playerId to Sever
        [HttpPost("{gameId}/set-starting-player/{playerId}")]
        public IActionResult SetStartingPlayer(int gameId, int playerId)
        {
            try
            {
                _gameService.GameSetStartingPlayer(gameId, playerId);
                return Ok(new { message = "Starting player set." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST place a tile on the board - Clent sends tile the player wnts to place on board
        [HttpPost("{gameId}/player-place-tile/{playerId}")]
        public IActionResult PlayerPlaceTile(int gameId, [FromBody] TilePlacementRequest request)
        {
            try
            {
                PlacementStatus status = _gameService.PlayerPlaceTile(gameId, request.PlayerId, request.Tile, request.X, request.Y);
                Game game = _gameService.GetGameById(gameId);

                //check for game end
                if (game.GameOver == true)
                {
                    return Ok(new
                    {
                        message = "Game over"
                    });
                }

                //return result of tile placent to Client
                return status switch
                {
                    PlacementStatus.Success => Ok(new { message = "Tile placed successfully!" }),
                    PlacementStatus.PositionOccupied => BadRequest(new { message = "That position is already occupied." }),
                    PlacementStatus.NoAdjacentTile => BadRequest(new { message = "The tile must be placed adjacent to another tile." }),
                    PlacementStatus.InvalidTile => BadRequest(new { message = "Tile must match the color or number of an adjacent tile." }),
                    _ => BadRequest(new { message = "An unknown error occurred." }),
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //POST pick up a tile from the TileDeck - Client sends request to pick up Tile from TileDeck
        [HttpPost("{gameId}/pick-up-tile")]
        public IActionResult PlayerPickUpTile(int gameId, [FromBody] PickUpTileRequest request)
        {
            try
            {
                _gameService.PlayerPickUpTile(gameId, request.PlayerId);

                return Ok(new { message = "Tile picked up successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //GET - Server sends state of current game objects to client for rendering
        [HttpGet("{gameId}/game-state")]
        public IActionResult GetGameState(int gameId)
        {
            try
            {
                Game game = _gameService.GetGameById(gameId);

                if (game == null)
                    return NotFound(new { message = $"Game not found" });

                //sent game info to front end
                return Ok(new
                {
                    gameId = game.GameId,
                    players = game.GetPlayers(),
                    hands = game.Hands,
                    tileDeck = game.TileDeck,
                    gameBoard = game.GameBoard,
                    currentScore = game.Score
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
