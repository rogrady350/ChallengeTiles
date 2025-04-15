namespace ChallengeTiles.Server.Models.DTO
{
    public class HandGameDTO
    {
        //data needed for displaying in-game hands
        public List<TileDTO> HandTiles { get; set; }
        public int HandId { get; internal set; }
    }
}
