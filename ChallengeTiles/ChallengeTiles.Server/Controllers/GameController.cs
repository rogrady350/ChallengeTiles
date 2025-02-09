using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Controllers
{
    /*Class purpose: Manage HTTP API end points for game actions
       start a game, make a move*/

    //Attributes - define routing
    [Route("api/[controller]")] //base URL route for controller
    [ApiController] //Marks class as an API controller

    //Handle route-specific logic
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService; //Service handling game logic (deck, hand, etc.)
        private readonly PlayerService _playerService;

        //Constructor creates game instance
        public GameController(GameService gameService, PlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;
        }

        //start a new game
        
        //API end point for Tile component
        [HttpGet("tiles")]
        public IActionResult GetTiles()
        {
            //hardcoded list for testing
            var tiles = new List<Tile>
            {
                new Tile(1, 1, "red"),
                new Tile(2, 2, "blue"),
                new Tile(3, 3, "green"),
                new Tile(4, 4, "yellow")
            };

            return Ok(tiles); //returns list in JSON format
        }
    }
}
