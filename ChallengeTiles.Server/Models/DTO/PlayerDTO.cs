namespace ChallengeTiles.Server.Models.DTO
{
    public class PlayerDTO
    {
        //data needed for displying Hands, organized by game
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public List<HandDTO> Hands { get; set; } //list of hands associated with gameId
    }
}
