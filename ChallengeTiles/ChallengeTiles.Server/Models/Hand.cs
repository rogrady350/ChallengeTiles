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
        public string GameId { get; set; } //forign key to Game Table

        public string TilesJson { get; private set; } //column for storing list of initial Tiles in Players Hand as JSON string

        [NotMapped] //EF Core ignores when mapping the table. Used for game play logic ONLY
        public List<Tile> HandTiles { get; private set; }

        //default no args constructor for EF (not used in game play logic)
        public Hand() { }

        //constructor for dealing a new hand (stores initial hand in DB for player to reference what hand they were dealt)
        public Hand(List<Tile> initialTiles)
        {
            HandTiles = initialTiles ?? new List<Tile>();
            TilesJson = JsonSerializer.Serialize(HandTiles); //only set once when hand is created
        }

        //game logic/api methods (do not update TilesJson in db)
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
