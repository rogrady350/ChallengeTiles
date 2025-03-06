namespace ChallengeTiles.Server.Models.DTO
{
    public class PlayerGameInfo
    {
        //data needed for displying Hands, organized by game
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public List<HandGameInfo> Hands { get; set; } //list of hands associated with gameId
    }
}
