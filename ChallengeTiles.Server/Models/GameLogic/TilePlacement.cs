namespace ChallengeTiles.Server.Models.GameLogic
{
    //class tracks tile position on board
    //Only expanding horizontally. No need for Y coordinate. Renamed "X" to "Position" for clarity.
    public class TilePlacement
    {
        //attributes, getters, setters
        public Tile Tile {  get; private set; }
        public int Position { get; private set; }

        //constructor
        public TilePlacement(Tile tile, int position)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile), "Tile cannot be null");

            Tile = tile;
            Position = position;
        }
    }
}
