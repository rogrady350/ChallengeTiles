using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace ChallengeTiles.Server.Models.GamePlay
{
    //Class holds game state and data
    public class Game
    {
        //default no args constructor for ef core
        public Game() { }

        //constructor with default values: calls other constructor with suggested values of 2 colors and 7 tiles per hand
        public Game(Player player1, Player player2) : this(2, 7, player1, player2) { }

        //main constructor with all game settings
        public Game(int numberOfColors, int numberOfTiles, Player player1, Player player2)
        {
            if (player1 == null || player2 == null)
            {
                throw new ArgumentNullException("Both players must be provided.");
            }

            NumberOfColors = numberOfColors;
            NumberOfTiles = numberOfTiles;

            //store player ID's
            Player1Id = player1.PlayerId;
            Player2Id = player2.PlayerId;

            //Create new game objcets
            GameBoard = new GameBoard();
            TileDeck = new TileDeck(numberOfColors);

            //Deal constructor builds and shuffles TileDeck (prepares deck to be dealt)
            Deal = new Deal(TileDeck);
        }

        //attributres, getters, setters
        [Key]
        public int GameId { get; private set; } //unique game ID. auto incremented in db

        //game settings
        public int NumberOfColors { get; private set; } //number of colors that will be used to play
        public int NumberOfTiles { get; private set; } //number of tiles each player gets dealt

        //player Foreign Keys (stored in DB)
        public int Player1Id { get; private set; }
        public int Player2Id { get; private set; }

        //navigation properties (FK's. EF uses these for relationships)
        [ForeignKey("Player1Id")]
        public Player Player1 { get; protected set; }

        [ForeignKey("Player2Id")]
        public Player Player2 { get; protected set; }

        //game play logic (not stored in db)
        [NotMapped]
        public GameBoard GameBoard { get; private set; }

        [NotMapped]
        public TileDeck TileDeck { get; private set; }

        [NotMapped]
        public Deal Deal { get; private set; }

        public override string ToString()
        {
            //use variabls with null check to avoid crashes if EF core doesn't load properties immediately
            string player1Name = Player1?.Name ?? "Unknown";
            string player2Name = Player2?.Name ?? "Unknown";

            //ToString
            return $"Game ID: {GameId}, Players: {player1Name} vs {player2Name}, Colors: {NumberOfColors}, Tiles: {NumberOfTiles}";
        }
    }
}
