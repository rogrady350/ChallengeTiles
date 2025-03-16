namespace ChallengeTiles.Server.Models.DTO
{
    public class TileDTO
    {
        //used to send Tile attributes to frontend (GET)
        public int TileId { get; set; }
        public int Number { get; set; }
        public string Color { get; set; }
        public string TileImageUrl { get; set; }
    }
}
