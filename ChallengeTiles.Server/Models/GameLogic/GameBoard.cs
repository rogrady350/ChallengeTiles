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
            Console.WriteLine($"GameBoard PlaceTile debug - Placing tile {tile.Id} at ({x}, {y})");
            PlacedTiles.Add(new TilePlacement(tile, x, y));
            Console.WriteLine("Tile successfully placed.");
        }

        /*tile placement validation. rules:
          adjacent tiles must be next to tile of same number OR color
          tiles must be placed directly on either side of tiles currently on board
          only called when player is placing a tile. starting tile automatically added to center during StartGame*/
        public PlacementStatus ValidatePlacement(Game game, int playerId, Tile tile, int x, int y)
        {
            Console.WriteLine($"GameBoard ValidatePlacement debug - Checking placement for Tile ID: {tile.Id}, Color: {tile.Color}, Number: {tile.Number}, Position: ({x}, {y})");
            //check if the position is already occupied
            if (PlacedTiles.Any(p => p.X == x && p.Y == y))
            {
                Console.WriteLine("Position already occupied.");
                return PlacementStatus.PositionOccupied;
            }

            //find value of TilePlacement positions to left, right, above, below
            TilePlacement? adjacentTile = PlacedTiles.FirstOrDefault(p => 
                (p.X == x - 1 && p.Y == y) || //left
                (p.X == x + 1 && p.Y == y) || //right
                (p.X == x && p.Y == y - 1) || //above
                (p.X == x && p.Y == y + 1));  //below;

            //check if position has a Tile in it
            if (adjacentTile == null)
            {
                Console.WriteLine("No adjacent tile found.");
                return PlacementStatus.NoAdjacentTile;
            }

            //if there is an adjacent tile, check if it matches color OR number
            if (adjacentTile.Tile.Color != tile.Color && adjacentTile.Tile.Number != tile.Number)
            {
                Console.WriteLine($"Invalid placement. Adjacent Tile ID: {adjacentTile.Tile.Id}, Color: {adjacentTile.Tile.Color}, Number: {adjacentTile.Tile.Number}");
                return PlacementStatus.InvalidTile;
            }

            //if all above checks pass, tile placement allowed
            Console.WriteLine("Placement successful.");
            return PlacementStatus.Success;
        }
    }
}
