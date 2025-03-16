namespace ChallengeTiles.Server.Models.DTO
{
    public class PlayerInfoDTO
    {
        //used to only send player id and name to front end
        /*revisit Hand, Player, Game DTO's intended for sending db stats for GameStats page
         may be able to simplify structure and/or reuse some DTO's
         need to look into best practice when ready to create GameStats page*/
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }
}
