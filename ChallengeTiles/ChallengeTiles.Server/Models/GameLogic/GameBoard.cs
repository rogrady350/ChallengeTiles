using ChallengeTiles.Server.Helpers;

namespace ChallengeTiles.Server.Models.GameLogic
{
    //Manages actions on board
    public class GameBoard
    {
        //attributes, getters, setters
        //list to hold tiles on board
        public List<TilePlacement> PlacedTiles { get; private set; }

        //constructor
        public GameBoard()
        {
            PlacedTiles = new List<TilePlacement>();
        }

        //place a tile on the board
        public void PlaceTile(Tile tile, int x, int y)
        {
            PlacedTiles.Add(new TilePlacement(tile, x, y));
        }

        /*tile placement validation. rules:
          adjacent tiles must be next to tile of same number OR color
          tiles must be placed directly on either side of tiles currently on board
          only called when player is placing a tile. starting tile automatically added to center during StartGame*/
        public PlacementStatus ValidatePlacement(Tile tile, int x, int y)
        {
            // Check if the position is already occupied
            if (PlacedTiles.Any(p => p.X == x && p.Y == y))
            {
                return PlacementStatus.PositionOccupied;
            }

            // Find value of TilePlacement positions to left, right, above, below
            TilePlacement? adjacentTile = PlacedTiles.FirstOrDefault(p => 
                (p.X == x - 1 && p.Y == y) || //left
                (p.X == x + 1 && p.Y == y) || //right
                (p.X == x && p.Y == y - 1) || //above
                (p.X == x && p.Y == y + 1));  //below;
            //check if position has a Tile in it
            if (adjacentTile == null)
            {
                return PlacementStatus.NoAdjacentTile;
            }

            //if there is an adjacent tile, check if it matches color OR number
            if (adjacentTile.Tile.Color != tile.Color && adjacentTile.Tile.Number != tile.Number)
            {
                return PlacementStatus.InvalidTile;
            }

            //if all above checks pass, tile placement allowed
            return PlacementStatus.Success;
        }
    }
}
