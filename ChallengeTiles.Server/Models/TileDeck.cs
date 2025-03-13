namespace ChallengeTiles.Server.Models
{
    public class TileDeck
    {
        //attributes
        public List<Tile> Tiles { get; private set; }   //List of tiles that will be available for play
        private readonly int _playedColorsCount; //number of colors being played (will be set by user when playing)
        private readonly List<string> availableColors = Constants.availableColors; //list of all colors that can be played

        //constants for min and max values of tiles
        private const int TileMin = Constants.TileMin;
        private const int TileMax = Constants.TileMax;

        //Deck constructor
        public TileDeck(int playedColorsCount)
        {
            Tiles = new List<Tile>(); //create list of tiles being played
            _playedColorsCount = playedColorsCount;
        }

        //method to populate deck. number of colors chosen can be set by user
        public void CreateTileDeck()
        {
            if (_playedColorsCount > availableColors.Count || _playedColorsCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_playedColorsCount), "Invalid number of colors selected.");
            }

            int id = 1; //id's start at 1

            foreach (var color in availableColors.Take(_playedColorsCount))
            {
                for (int number = TileMin; number <= TileMax; number++)
                {
                    Tiles.Add(new Tile(id, number, color));
                    id++;
                }
            }
        }

        //shuffle tiles
        public void ShuffleTiles()
        {
            Tile tempTile;
            int randomIndex;

            for (int i = 0; i < Tiles.Count; i++)
            {
                randomIndex = Random.Shared.Next(Tiles.Count);

                tempTile = Tiles[randomIndex];
                Tiles[randomIndex] = Tiles[i];
                Tiles[i] = tempTile;
            }
        }

        //return a single Tile object
        public Tile GetTile(int index)
        {
            if (index < 0 || index >= Tiles.Count)
                throw new ArgumentOutOfRangeException($"{index} out of bounds");

            return Tiles[index];
        }


        //return total number of tiles being used
        public int GetTileCount()
        {
            return Tiles.Count;
        }

        //Return all tiles of a specified color
        public List<Tile> FindTilesByColor(string color)
        {
            return Tiles.Where(t => t.Color == color).ToList();
        }

        //return all tiles of a specified number
        public List<Tile> FindTilesByNumber(int number)
        {
            return Tiles.Where(t => t.Number == number).ToList();
        }

        //remove a tile
        public Tile RemoveTile(int index)
        {
            if (index < 0 || index >= Tiles.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }
            Tile tile = Tiles[index];
            Tiles.RemoveAt(index);

            return tile;
        }

        //reset deck
        public void ResetTileDeck()
        {
            Tiles.Clear();
            CreateTileDeck();
            ShuffleTiles();
        }

        //ToString
        public override string ToString()
        {
            var colorSummary = Tiles
                .GroupBy(t => t.Color)
                .Select(g => $"{g.Key}: {g.Count()} tiles")
                .ToList();

            string tileList = string.Join(", ", Tiles.Select(t => t.ToString()));
                
            return $"TileDeck with {Tiles.Count} tiles\nTiles: {tileList}";
        }
    }
}
