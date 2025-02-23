using Microsoft.EntityFrameworkCore;
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
        //default no args constructor for EF
        public Game() 
        {
           //initialize collection - ensure EF core handles the Game - Hand relationship
            Hands = new List<Hand>();
        }

        //main constructor with all game settings
        public Game(int numberOfColors, int numberOfTiles)
        {
            NumberOfColors = numberOfColors;
            NumberOfTiles = numberOfTiles;

            //Create new game objcets
            GameBoard = new GameBoard();
            TileDeck = new TileDeck(numberOfColors);

            //initialize collection
            Hands = new List<Hand>();

            //Deal constructor builds and shuffles TileDeck (prepares deck to be dealt)
            Deal = new Deal(TileDeck);

            //Initial score starts at 0
            Score = 0;
        }

        //attributres, getters, setters
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; private set; } //unique game ID. auto incremented in db

        [Column]
        public int Score { get; set; }

        //1:n relationship with Hand: Each Game has multiple Hands (one per player)
        public List<Hand> Hands { get; set; }

        //game settings
        public int NumberOfColors { get; set; } //number of colors that will be used to play
        public int NumberOfTiles { get; set; } //number of tiles each player gets dealt

        //game play logic (not stored in db)
        [NotMapped]
        public GameBoard GameBoard { get; private set; }

        [NotMapped]
        public TileDeck TileDeck { get; private set; }

        [NotMapped]
        public Deal Deal { get; private set; }

        //get players playing this game: extracts player from respective hand and adds to list of players playing
        public List<Player> GetPlayers()
        {
            return Hands.Select(h => h.Player).ToList();
        }

        //add players to the game through hand: takes list of players, number of tiles chosen for the hand, and the Deck of tiles being played with
        public void AddPlayers(List<Player> players, int tilesPerPlayer, TileDeck tileDeck)
        {
            foreach (var player in players)
            {
                var initialHand = new List<Tile>(); //create an initially empty Hand
                Hands.Add(new Hand(player.PlayerId, this.GameId, initialHand)); //assign Player and Game FK's and create record with empty hand
            }
        }

        //alternate dealing tiles to players
        public void DealTiles(int totalTilesToDeal)
        {
            int currentPlayerIndex = 0; //set index to first player initially
            for (int i = 0; i < totalTilesToDeal; i++)
            {
                //get the current player (alternating through the players in the Hands list)
                var currentPlayer = Hands[currentPlayerIndex].Player;

                //call DealTile from the Deal class to deal one tile to the current player's hand
                Deal.DealTile(currentPlayer, this.GameId);

                //alternate to the next player
                currentPlayerIndex = (currentPlayerIndex + 1) % Hands.Count;
            }
        }

        //method to call DealTile, also will update the score


        //update score function
        public void UpdateScore(int points)
        {
            Score += points;
        }

        //ToString
        public override string ToString()
        {
            return $"Game ID: {GameId}, Colors: {NumberOfColors}, Tiles: {NumberOfTiles}";
        }
    }
}
