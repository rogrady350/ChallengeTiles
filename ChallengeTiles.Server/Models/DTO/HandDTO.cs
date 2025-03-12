namespace ChallengeTiles.Server.Models.DTO
{
    public class HandDTO
    {
        //data needed for displying Hands, organized by game
        public int HandId { get; set; }
        public string Tiles { get; set; } //JSON string of initial tiles associated with hand
    }
}
