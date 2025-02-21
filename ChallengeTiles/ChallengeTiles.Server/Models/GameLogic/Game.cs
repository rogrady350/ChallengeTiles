using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace ChallengeTiles.Server.Models.GameLogic
{
    //Class holds game state and data
    public class Game
    {
        //default no args constructor for ef core
        public Game() { }

        //main constructor with all game settings
        public Game(int numberOfColors, int numberOfTiles)
        {
            NumberOfColors = numberOfColors;
            NumberOfTiles = numberOfTiles;

            //Create new game objcets
            GameBoard = new GameBoard();
            TileDeck = new TileDeck(numberOfColors);

            //Deal constructor builds and shuffles TileDeck (prepares deck to be dealt)
            Deal = new Deal(TileDeck);

            //hands added when players join. Game table connected to Player table through Hand table
            Hands = new List<Hand>();
        }

        //attributres, getters, setters
        [Key]
        public int GameId { get; private set; } //unique game ID. auto incremented in db

        //1:n relationship with Hand: Each Game has multiple Hands (one per player)
        public List<Hand> Hands { get; set; }

        //game settings
        public int NumberOfColors { get; private set; } //number of colors that will be used to play
        public int NumberOfTiles { get; private set; } //number of tiles each player gets dealt

        //game play logic (not stored in db)
        [NotMapped]
        public GameBoard GameBoard { get; private set; }

        [NotMapped]
        public TileDeck TileDeck { get; private set; }

        [NotMapped]
        public Deal Deal { get; private set; }

        public override string ToString()
        {
            //ToString
            return $"Game ID: {GameId}, Colors: {NumberOfColors}, Tiles: {NumberOfTiles}";
        }
    }
}
