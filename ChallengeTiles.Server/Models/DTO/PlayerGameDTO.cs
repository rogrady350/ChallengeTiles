namespace ChallengeTiles.Server.Models.DTO
{
    public class PlayerGameDTO
    {
        //used to only send player id and name to frontend for in-game play
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }
}
