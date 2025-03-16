namespace ChallengeTiles.Server.Models.DTO
{
    public class HandGameDTO
    {
        //data needed for displaying Initial hands stored in db
        public int HandId { get; set; }
        public List<TileDTO> HandTiles { get; set; }
    }
}
