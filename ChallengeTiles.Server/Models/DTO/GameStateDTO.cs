namespace ChallengeTiles.Server.Models.DTO
{
    public class GameStateDTO
    {
        //used to send aspects of the current state of the game to the front end (GET)
        public int GameId { get; set; }
        public List<PlayerDTO> Players { get; set; }
        public List<GameHandDTO> Hands { get; set; }
        public List<TileDTO> TileDeck { get; set; }
        public List<TilePlacementDTO> GameBoard { get; set; }
        public int CurrentScore { get; set; }
    }
}
