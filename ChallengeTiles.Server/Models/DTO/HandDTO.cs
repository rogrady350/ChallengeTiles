namespace ChallengeTiles.Server.Models.DTO
{
    public class HandDTO
    {
        //data needed for displying Hands in stats table, organized by game
        public int HandId { get; set; }
        public string Tiles { get; set; } //JSON string of initial tiles associated with hand
    }
}
