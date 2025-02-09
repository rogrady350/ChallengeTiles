using ChallengeTiles.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeTiles.Server.Controllers
{
    /*Class purpose: Manage HTTP API end points for player data
      profile, history, etc*/

    //Attributes - define routing
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly MongoDbContext? _mongoContext;
        private readonly MysqlDbContext? _mysqlContext;

        //constructor. DbContext injected by ASP.Net
        public PlayerController(MongoDbContext? mongoContext, MysqlDbContext? mysqlContext)
        {
            //assign context to use throughout controller
            _mongoContext = mongoContext;
            _mysqlContext = mysqlContext;
        }
    }
}
