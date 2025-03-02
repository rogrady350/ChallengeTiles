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

    //swagger test values
    ///need to look more into how to get this to work. swagger not seeing values. crashed when updating Program.cs to see these
    public class StartGameRequestExample : IExamplesProvider<StartGameRequest>
    {
        public StartGameRequest GetExamples()
        {
            return new StartGameRequest
            {
                PlayerIds = new List<int> { 1, 2 },
                NumberOfColors = 2,
                NumberOfTiles = 7
            };
        }
    }
}
