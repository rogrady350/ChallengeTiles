namespace ChallengeTiles.Server.Models.DTO
{
    public class GameStateDTO
    {
        //used to send aspects of the current state of the game to the front end (GET)
        public int GameId { get; set; }
        public List<PlayerGameDTO> Players { get; set; }
        public List<HandGameDTO> Hands { get; set; }
        public List<TileDTO> TileDeck { get; set; }
        public List<TilePlacementDTO> GameBoard { get; set; }
        public int CurrentScore { get; set; }
        public int? CurrentPlayerId { get; set; }
        //public string TileDeckImageUrl { get; set; }
        public bool GameOver { get; set; }
    }
}
