namespace ChallengeTiles.Server.Models
{
    public class Hand
    {
        //list tiles player has
        public List<Tile> HandTiles { get; private set; }

        //default constructor
        public Hand()
        {
            HandTiles = new List<Tile>();
        }

        //constructor with list of tiles being played
        public Hand(Tile[] playedTiles)
        {
            HandTiles = new List<Tile>();

            //add tiles from tiles being played to players hand
            HandTiles.AddRange(playedTiles);
        }

        //add a single tile to players hand
        public void AddTile(Tile playedTile)
        {
            HandTiles.Add(playedTile);
        }

        //add multiple tiles
        public void AddTile(Tile[] playedTiles)
        {
            HandTiles.AddRange(playedTiles);
            //?? AddTile(new[] { playedTile });
        }

        //place a tile in specified positions (allows players to organize tiles)
        public void PlaceTile(Tile playedTile, int index)
        {
            if (index < 0 || index >= HandTiles.Count)
            {
                throw new ArgumentOutOfRangeException($"{index} out of range");
            }

            HandTiles[index] = playedTile;
        }

        //take a tile from a specified position
        public Tile GetTile(int index)
        {
            if (index < 0 || index >= HandTiles.Count)
            {
                throw new ArgumentOutOfRangeException($"{index} out of range");
            }

            return HandTiles[index];
        }

        //remove a tile from a specified position
        public Tile RemoveTile(int index)
        {
            if (index < 0 || index >= HandTiles.Count)
            {
                throw new ArgumentOutOfRangeException($"{index} out of range");
            }

            Tile removedTile = HandTiles[index];
            HandTiles.RemoveAt(index);
            return removedTile;
        }

        //ToString
        public override string ToString()
        {
            return $"{string.Join(",", HandTiles)}";
        }
    }
}
