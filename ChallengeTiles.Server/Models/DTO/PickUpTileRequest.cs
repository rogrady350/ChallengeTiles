using ChallengeTiles.Server.Models.GameLogic;

namespace ChallengeTiles.Server.Models.DTO
{
    public class PickUpTileRequest
    {
        //GameId sent in URL in RESTful API's, have for added flexibility
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }
}
