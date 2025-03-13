namespace ChallengeTiles.Server.Models.DTO
{
    public class GameHandDTO
    {
        //data needed for rendering hand with using list that is updated during game play
        public int HandId { get; set; }
        public List<TileDTO> HandTiles { get; set; }
    }
}
