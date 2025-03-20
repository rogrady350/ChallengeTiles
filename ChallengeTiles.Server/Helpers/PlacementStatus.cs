namespace ChallengeTiles.Server.Helpers
{
    //named values for tile placement responses
    public enum PlacementStatus
    {
        Success,          //valid placement
        PositionOccupied, //position has a tile in it
        NoAdjacentTile,   //no tile next to attempted placement
        InvalidTile,      //color or number don't match
        NoHand,           //ensure player has valid hand
        TileNotFound      //tile not in players hand
    }
}