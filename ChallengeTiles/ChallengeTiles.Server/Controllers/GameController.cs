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
                Game game = _gameService.StartNewGame(request.PlayerIds, request.NumberOfColors, request.NumberOfTiles);

                //return response with game data
                return Ok(new
                {
                    gameId = game.GameId,
                    players = game.GetPlayers(),
                    message = "Please select a starting player" //send message to ask user to select starting player.
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //POST for setting the starting player: Client sends gameId selected playerId to Sever
        [HttpPost("{gameId}/set-starting-player/{playerId}")]
        public IActionResult SetStartingPlayer(int gameId, int playerId)
        {
            try
            {
                _gameService.GameSetStartingPlayer(gameId, playerId);
                return Ok("Starting player set.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST place a tile on the board
        public IActionResult PlaceTile(int gameId, [FromBody] TilePlacementRequest request)
        {
            try
            {
                PlacementStatus status = _gameService.PlayerPlaceTile(gameId, request.PlayerId, request.Tile, request.X, request.Y);

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

        //POST pick up a tile from the TileDeck
        [HttpPost("{gameId}/pick-up-tile")]
        public IActionResult PickUpTile(int gameId, [FromBody] PickUpTileRequest request)
        {
            _gameService.PlayerPickUpTile(gameId, request.PlayerId);
        }
    }
}
