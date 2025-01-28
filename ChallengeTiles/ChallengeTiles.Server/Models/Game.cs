using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace ChallengeTiles.Server.Models
{
    //Class holds game state and data
    public class Game
    {
        private static int idCounter = 1;

        //default constructor with suggested values
        public Game()
        {
            Id = GenerateId();
            NumberOfColors = 2;
            NumberOfTiles = 7;
        }

        //constructor
        public Game(int numberOfColors, int numberOfTiles)
        {
            Id = GenerateId();
            NumberOfColors = numberOfColors;
            NumberOfTiles = numberOfTiles;
        }

        //auto generate player id
        private static int GenerateId()
        {
            return idCounter++;
        }

        //attributres, getters, setters
        public int Id { get; set; }
        public int NumberOfColors { get; set; } //number of colors that will be used to play
        public int NumberOfTiles { get; set; } //number of tiles each player gets dealt
        public List<Player> Players { get; set; }
        public TileDeck TileDeck { get; set; }
        public string GameStatus { get; set; } //switch from InProgress to complete when all tiles are on board
    }
}
