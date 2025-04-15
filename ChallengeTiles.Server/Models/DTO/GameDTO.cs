namespace ChallengeTiles.Server.Models.DTO
{
    public class GameDTO
    {
        //used to send Game data stored in database to frontend
        public int GameId { get; set; }
        public int Score {  get; set; }
        public int NumberOfColors { get; set; }
        public int NumberOfTiles { get; set; }
        public List<PlayerDTO> Players { get; set; } //list of Players associated with Game through Hand table
    }
}
