namespace ChallengeTiles.Server.Models
{
    public class TileDeck
    {
        //attributes
        private readonly List<Tile> _tiles;   //List of tiles that will be available for play
        private readonly int _playedColorsCount; //number of colors being played (will be set by user when playing)

        //list of all colors that can be played
        private readonly List<string> availableColors = Constants.availableColors;

        //constants for min and max values of tiles
        private const int TileMin = Constants.TileMin;
        private const int TileMax = Constants.TileMax;

        //Deck constructor
        public TileDeck(int playedColorsCount)
        {
            _tiles = []; //create array of tiles being played
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
                    _tiles.Add(new Tile(id, number, color));
                    id++;
                }
            }
        }

        //shuffle tiles
        public void ShuffleTiles()
        {
            Tile tempTile;
            int randomIndex;

            for (int i = 0; i < _tiles.Count; i++)
            {
                randomIndex = Random.Shared.Next(_tiles.Count);

                tempTile = _tiles[randomIndex];
                _tiles[randomIndex] = _tiles[i];
                _tiles[i] = tempTile;
            }
        }

        //return a single Tile object
        public Tile GetTile(int index)
        {
            if (index < 0 || index >= _tiles.Count)
                throw new ArgumentOutOfRangeException($"{index} out of bounds");

            return _tiles[index];
        }

        //return all tiles
        public List<Tile> GetTiles()
        {
            return _tiles;
        }

        //return total number of tiles being used
        public int GetTileCount()
        {
            return _tiles.Count;
        }

        //Return all tiles of a specified color
        public List<Tile> FindTilesByColor(string color)
        {
            return _tiles.Where(t => t.Color == color).ToList();
        }

        //return all tiles of a specified number
        public List<Tile> FindTilesByNumber(int number)
        {
            return _tiles.Where(t => t.Number == number).ToList();
        }

        //remove a tile
        public Tile RemoveTile(int index)
        {
            if (index < 0 || index >= _tiles.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }
            Tile tile = _tiles[index];
            _tiles.RemoveAt(index);

            return tile;
        }

        //reset deck
        public void ResetTileDeck()
        {
            _tiles.Clear();
            CreateTileDeck();
            ShuffleTiles();
        }

        //ToString
        public override string ToString()
        {
            var colorSummary = _tiles
                .GroupBy(t => t.Color)
                .Select(g => $"{g.Key}: {g.Count()} tiles")
                .ToList();

            string tileList = string.Join(", ", _tiles.Select(t => t.ToString()));

            return $"TileDeck with {_tiles.Count} tiles\nTiles: {tileList}";
        }
    }
}
