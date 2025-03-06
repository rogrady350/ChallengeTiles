namespace ChallengeTiles.Server.Models.DTO
{
    public class GameInfo
    {
        //data needed for displying info organized by game
        public int GameId { get; set; }
        public List<PlayerGameInfo> Players { get; set; } //list of Hands associated with Game
    }
}
