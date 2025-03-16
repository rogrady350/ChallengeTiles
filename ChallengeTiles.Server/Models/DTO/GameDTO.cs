namespace ChallengeTiles.Server.Models.DTO
{
    public class GameDTO
    {
        //used to send Game data stored in db to frontend
        public int GameId { get; set; }
        public List<PlayerDTO> Players { get; set; } //list of Players and their respective intial Hands associated with Game
    }
}
