namespace ChallengeTiles.Server.Models.DTO
{
    public class TilePlacementDTO
    {
        //used to send TilePlacement info to render Tile on board to frontend (GET)
        public TileDTO Tile { get; set; }
        public int Position { get; set; }
    }
}
