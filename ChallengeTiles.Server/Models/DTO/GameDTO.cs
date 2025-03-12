namespace ChallengeTiles.Server.Models.DTO
{
    public class GameDTO
    {
        public int GameId { get; set; }
        public List<PlayerDTO> Players { get; set; } //list of Hands associated with Game
    }
}
