using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
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

        [HttpPost("start")]
        public ActionResult<Game> StartNewGame(int numberOfColors, int numberOfTiles)
        {
            var game = _gameService.StartNewGame(numberOfColors, numberOfTiles); //start new game
            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game); //return created game
        }

        [HttpGet("{id}")]
        public ActionResult<Game> GetGame(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

    }
}
