using Swashbuckle.AspNetCore.Filters;

namespace ChallengeTiles.Server.Models.DTO
{
    public class StartGameRequest
    {
        /*ASP.NET automatically maps the JSON keys to these properties when a POST requests sends JSON
         no consturctor is needed*/
        public List<int> PlayerIds { get; set; }
        public int NumberOfColors { get; set; }
        public int NumberOfTiles { get; set; }
    }
}
