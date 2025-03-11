namespace ChallengeTiles.Server.Models.DTO
{
    public class GameInfo
    {
        public int GameId { get; set; }
        public List<PlayerGameInfo> Players { get; set; } //list of Hands associated with Game
    }
}
