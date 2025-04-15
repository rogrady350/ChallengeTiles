namespace ChallengeTiles.Server.Models.DTO
{
    public class PlayerDTO
    {
        //data needed from db for displying player names and initial hands associated with player
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public List<HandDTO> Hands { get; set; } //list of hands associated with gameId
    }
}
