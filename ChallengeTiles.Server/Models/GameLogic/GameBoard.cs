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

        //place a tile on the board (now can only expand horizontally)
        public void PlaceTile(Tile tile, int position)
        {
            PlacedTiles.Add(new TilePlacement(tile, position));
        }

        /*tile placement validation. rules:
          adjacent tiles must be next to tile of same number OR color
          tiles must be placed directly on either side of tiles currently on board
          only called when player is placing a tile. starting tile automatically added to center during StartGame*/
        public PlacementStatus ValidatePlacement(Game game, int playerId, Tile tile, int position)
        {
            //check if the position is already occupied
            if (PlacedTiles.Any(p => p.Position == position))
            {
                return PlacementStatus.PositionOccupied;
            }

            //find value of TilePlacement positions to left, right, above, below
            TilePlacement? adjacentTile = PlacedTiles.FirstOrDefault(p => 
                (p.Position == position - 1) || //left
                (p.Position == position + 1));  //right;

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

        //Track allowed positions to highlight on front end
        public List<int> GetAllowedPositions()
        {
            if (PlacedTiles.Count == 0)
                return new List<int> { 0 }; //only center position allowed if no tiles on board

            //outward most occupied positions
            int min = PlacedTiles.Min(p => p.Position);
            int max = PlacedTiles.Max(p => p.Position);

            return new List<int> { min-1, max+1 };
        }
    }
}
