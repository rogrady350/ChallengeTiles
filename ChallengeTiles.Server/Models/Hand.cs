using ChallengeTiles.Server.Models.GameLogic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ChallengeTiles.Server.Models
{
    public class Hand
    {
        //list of tiles player has
        //default no args constructor for EF (not used in game play logic)
        public Hand()
        {
            HandTiles = new List<Tile>();
        }

        //constructor for creating a new hand (stores initial hand in DB for player to reference what hand they were dealt)
        public Hand(int playerId, int gameId, List<Tile> initialTiles)
        {
            PlayerId = playerId;
            GameId = gameId;
            HandTiles = initialTiles ?? new List<Tile>(); //list of tiles in Hand maintained througout game play
            Tiles = JsonSerializer.Serialize(HandTiles); //only set once when hand is created
        }

        //attributes, getters, setters
        [Key]
        public int HandId { get; set; }  //primary key in Hand table

        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; } //foreign key to Player table
        [ForeignKey("GameId")]
        public int GameId { get; set; } //forign key to Game Table

        //navigation properties: non-nullable - every Hand must be associated with a Player and a Game
        public Player Player { get; set; }
        public Game Game { get; set; }

        [Column]
        public string Tiles { get; set; } //column for storing list of initial Tiles in Players Hand as JSON string

        [NotMapped] //EF Core ignores when mapping the table. Used for game play logic ONLY
        public List<Tile> HandTiles { get; private set; } = new List<Tile>();

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
