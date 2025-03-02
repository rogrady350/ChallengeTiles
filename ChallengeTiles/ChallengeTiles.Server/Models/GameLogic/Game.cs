using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace ChallengeTiles.Server.Models.GameLogic
{
    //Class holds data and logic
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
            TileDeck = new TileDeck(numberOfColors);
            GameBoard = new GameBoard();

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

        [NotMapped]
        public int CurrentPlayerId { get; private set; } //keep track of turn

        [NotMapped]
        public bool GameOver { get; private set; } = false; //flag for game status

        //dictionary for holding player hands. Allows O(1) looup time
        private readonly Dictionary<int, Hand> _playerHands = new Dictionary<int, Hand>();  

        //game play functions
        //get players playing this game: extracts player from respective hand and adds to list of players playing
        public List<Player> GetPlayers()
        {
            return Hands.Select(h => h.Player).ToList(); //returns Player Object
        }

        //add players to the game through hand: takes list of players, number of tiles chosen for the hand, and the Deck of tiles being played with
        public void AddPlayers(List<Player> players, int tilesPerPlayer, TileDeck tileDeck)
        {
            foreach (Player player in players)
            {
                var initialHand = new List<Tile>(); //create an initially empty Hand

                //assign Player and Game FK's and with empty hand
                Hand hand = new Hand(player.PlayerId, this.GameId, initialHand)
                {
                    Player = player //assign the Player object
                };

                Hands.Add(hand); //add Hand to the Game's list of Hands
                _playerHands[player.PlayerId] = hand; //add Hand to dictionary
            }
        }

        //alternate dealing tiles to players
        public void DealTiles(int totalTilesToDeal)
        {
            int currentPlayerIndex = 0; //set index to first player initially
            for (int i = 0; i < totalTilesToDeal; i++)
            {
                //get the current player (alternating through the players in the Hands list)
                Player currentPlayerDeal = Hands[currentPlayerIndex].Player;

                //call DealTile from the Deal class to deal one tile to the current player's hand
                Deal.DealTile(currentPlayerDeal, this.GameId);

                //alternate to the next player
                currentPlayerIndex = (currentPlayerIndex + 1) % Hands.Count;
            }

            //add next tile in TileDeck to center of board
            Tile tile = TileDeck.RemoveTile(0);
            GameBoard.PlaceTile(tile, 0, 0);
        }

        //stores the player selected to make first move. Instantiate CurrentPlayerId
        public void SetStartingPlayer(int playerId)
        {
            if (!Hands.Any(h => h.PlayerId == playerId))
            {
                throw new InvalidOperationException($"Player {playerId} is not in this game.");
            }

            CurrentPlayerId = playerId;
        }

        //DrawlTile and UpdateScore
        public void PickUpTile(int playerId)
        {
            //find hand object associated with player
            if (!_playerHands.TryGetValue(playerId, out Hand? playerHand))
                if (playerHand == null)
            {
                throw new InvalidOperationException($"Player {playerId} does not have a hand in this game.");
            }

            Deal.DrawTile(playerHand.Player, this.GameId);
            Score++;
        }

        //place Tile on GameBoard
        public PlacementStatus PlaceTile(int playerId, Tile tile, int x, int y)
        {
            //find hand object associated with player
            if (!_playerHands.TryGetValue(playerId, out Hand? playerHand))
                throw new InvalidOperationException($"No hand found for Player {playerId}.");

            int tileIndex = playerHand.HandTiles.FindIndex(t => t == tile);
            if (tileIndex < 0)
                throw new InvalidOperationException($"{tileIndex} invalid");

            //validate chosen position
            PlacementStatus validationResult = GameBoard.ValidatePlacement(tile, x, y);
            if (validationResult != PlacementStatus.Success)
            {
                return validationResult; // Return the failure reason
            }

            //remove tile from players hand and place on board
            playerHand.HandTiles.RemoveAt(tileIndex);
            GameBoard.PlaceTile(tile, x, y);

            return PlacementStatus.Success;
        }

        //Turn Handling
        public void NextTurn()
        {
            //find index of current player from Hands list
            int currentIndex = Hands.FindIndex(h => h.PlayerId == CurrentPlayerId);

            //safety check for invalid CurrentPlayerId
            if (currentIndex == -1)
            {
                throw new InvalidOperationException("Current PlayerId invalid");
            }

            //get next player index
            int nextIndex = (currentIndex + 1) % Hands.Count;

            //set iterations to 0 at start of while loop. used to prevent infinite loop
            int iterations = 0;
            //find next player with tiles (while loop chosen in anticipation of being able to play with more than 2 players in future)
            while (Hands[nextIndex].HandTiles.Count == 0 && iterations < Hands.Count)
            {
                nextIndex = (nextIndex + 1) % Hands.Count; //move index to next player (moves back to current player if no other player has tiles)
                iterations++; //increment iterations to only loop the amount of times as there are players
            }

            //if all players hands have been checked and all found to have no tiles left, end game
            if (Hands[nextIndex].HandTiles.Count == 0)
            {
                Console.WriteLine("Game has ended");
                GameOver = true; //switch flag to true to signify end of game
                return; //game over. stop further execution.
            }

            CurrentPlayerId = Hands[nextIndex].PlayerId;
        }

        //ToString
        public override string ToString()
        {
            return $"Game ID: {GameId}, Colors: {NumberOfColors}, Tiles: {NumberOfTiles}";
        }
    }
}
