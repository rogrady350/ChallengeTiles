using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.DTO;
using ChallengeTiles.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Controllers
{
    /*Class purpose: Manage HTTP API end points for player data
      profile, history, etc*/

    //Attributes - define routing
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        //need to abstract - repository takes dbcontext, controller takes a service as a dpendency
        private readonly PlayerService _playerService;

        //constructor
        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        //POST registers a new player
        [HttpPost("register-player")]
        public IActionResult RegisterNewPlayer([FromBody] RegisterPlayerRequest request)
        {
            //validate requirements. return error message from Register player request if invalid input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _playerService.RegisterProfile(request.UserName, request.Password, request.Name, request.Email);

            if(!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //GET single player by id (not needed at this time)


        //GET retrieve list of players (Server side for GameSetupService.js getPlayers)
        //service returns IEnumerable, client specifies if it needs a list for indexing in query param
        [HttpGet("players")]
        public ActionResult<IEnumerable<Player>> GetPlayers([FromQuery] bool asList = false)
        {
            var players = _playerService.GetAllPlayers();

            //convert to list if requested
            if (asList)
            {
                return Ok(players.ToList());
            }

            return Ok(players);
        }

        //no services created to update(PUT) or delete (DELETE). Repository methods available for future use
    }
}