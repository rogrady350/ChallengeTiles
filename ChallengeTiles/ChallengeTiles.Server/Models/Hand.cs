using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ChallengeTiles.Server.Models
{
    public class Hand
    {
        //list tiles player has
        //attributes, getters, setters
        [Key]
        public int HandId { get; set; }  //primary key in Hand table
        public string PlayerId { get; set; } //foreign key to Player table
        public string GameId { get; set; } //forign key to Player Table

        public string TilesJson { get; private set; } //column for storing list of Tiles in Players Hand as JSON string

        [NotMapped] //EF Core ignores when mapping the table. Uses gets/sets when reading writing
        public List<Tile> HandTiles
        {
            get => string.IsNullOrEmpty(TilesJson) ? new List<Tile>() : JsonSerializer.Deserialize<List<Tile>>(TilesJson); //convert from JSON string back to list of Tiles
            set => TilesJson = JsonSerializer.Serialize(value); //converts list of Tiles to JSON string
        }

        //constructor with list of tiles being played. used when creating hand object which is empty before deal
        public Hand()
        {
            HandTiles = new List<Tile>();
        }

        //Constructor that sets TilesJson.
        public Hand(List<Tile> tiles)
        {
            HandTiles = tiles;
        }

        //add a single tile to players hand
        public void AddTile(Tile playedTile)
        {
            HandTiles.Add(playedTile);
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
