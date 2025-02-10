using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace ChallengeTiles.Server.Models
{
    //Class holds game state and data
    public class Game
    {
        //default constructor: calls other constructor with suggested values of 2 colors and 7 tiles per hand
        public Game(Player player1, Player player2) : this(2, 7, player1, player2) { }

        //constructor
        public Game(int numberOfColors, int numberOfTiles, Player player1, Player player2)
        {
            if (player1 == null || player2 == null)
            {
                throw new ArgumentNullException("Both players must be provided.");
            }

            Id = Guid.NewGuid().ToString(); //Generate GUID
            NumberOfColors = numberOfColors;
            NumberOfTiles = numberOfTiles;
            Players = new List<Player> { player1, player2 };
            
            //Create new game objcets
            GameState = new GameState();
            GameBoard = new GameBoard();

            //Create a TileDeck
            TileDeck = new TileDeck(numberOfColors);

            //Deal constructor builds and shuffles TileDeck (does not deal to players)
            Deal = new Deal(TileDeck);
        }

        //attributres, getters, setters
        public string Id { get; private set; } //unique game ID
        public int NumberOfColors { get; private set; } //number of colors that will be used to play
        public int NumberOfTiles { get; private set; } //number of tiles each player gets dealt
        public List<Player> Players { get; private set; }
        public GameState GameState { get; private set; }
        public GameBoard GameBoard { get; private set; }
        public TileDeck TileDeck { get; private set; }
        public Deal Deal { get; private set; }

        public override string ToString()
        {
            return $"Game ID: {Id}, Players: {Players[0].Name} vs {Players[1].Name}, Colors: {NumberOfColors}, Tiles: {NumberOfTiles}";
        }
    }
}
