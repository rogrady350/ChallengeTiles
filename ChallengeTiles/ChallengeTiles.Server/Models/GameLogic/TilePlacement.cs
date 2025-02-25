namespace ChallengeTiles.Server.Models.GameLogic
{
    //class tracks tile position on board
    public class TilePlacement
    {
        //attributes, getters, setters
        public Tile Tile {  get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        //constructor
        public TilePlacement(Tile tile, int x, int y)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile), "Tile cannot be null");

            Tile = tile;
            X = x;
            Y = y;
        }
    }
}
