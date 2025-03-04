using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
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
            
        }

        
    }
}