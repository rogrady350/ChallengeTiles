using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Controllers
{
    //Attributes - define routing
    [Route("api/[controller]")] //base URL route for controller
    [ApiController] //Marks class as an API controller

    //Handle route-specific logic
    public class GameController : ControllerBase
    {
        private readonly MongoDbContext? _mongoContext;
        private readonly MysqlDbContext? _mysqlContext;

        //constructor. DbContext injected by ASP.Net
        public GameController(MongoDbContext? mongoContext, MysqlDbContext? mysqlContext)
        {
            //assign context to use throughout controller
            _mongoContext = mongoContext;
            _mysqlContext = mysqlContext;
        }

        
    }
}
