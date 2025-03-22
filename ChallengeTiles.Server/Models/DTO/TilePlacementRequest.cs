namespace ChallengeTiles.Server.Models.DTO
{
    public class TilePlacementRequest
    {
        /*used to recieve request for placing a Tile on the board from frontend (POST)
          GameId sent in URL in RESTful API's, have for added flexibility*/
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public TileDTO Tile { get; set; } //use TileDTO rather than Tile to recieve Tile data from frontend
        public int Position { get; set; }
    }
}
