using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
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
           //initialize objects
            Hands = new List<Hand>();
            GameBoard = new GameBoard();
            TileDeck = new TileDeck(2);
            Deal = new Deal(TileDeck);
            PlayerHands = new Dictionary<int, Hand>();
            Score = 0;
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

            //initialize dictionary for hands to be updated during game play
            PlayerHands = new Dictionary<int, Hand>();

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
        [NotMapped]
        public Dictionary<int, Hand> PlayerHands { get; private set; } = new();

        //game play functions
        //get players playing this game: extracts player from respective hand and adds to list of players playing
        public List<Player> GetPlayers()
        {
            return Hands.Select(h => h.Player).ToList(); //returns Player Object
        }

        //add players to the game through hand: takes list of players, number of tiles chosen for the hand, and the Deck of tiles being played with
        //make sure it is only modifying in-memory data.
        public void AddPlayers(List<Player> players, int tilesPerPlayer, TileDeck tileDeck)
        {
            Console.WriteLine($"Game AddPlayers debug - Adding {players.Count} players");

            foreach (Player player in players)
            {
                //remove database lookup - only modify in memory data
                if (!PlayerHands.ContainsKey(player.PlayerId))
                {
                    Console.WriteLine($"Creating new hand object for Player {player.PlayerId} in Game {this.GameId}");
                    var initialHand = new List<Tile>(); //create an initially empty Hand

                    //assign Player and Game FK's and with empty hand
                    Hand hand = new Hand(player.PlayerId, this.GameId, initialHand)
                    {
                        Player = player //assign the Player object
                    };

                    PlayerHands[player.PlayerId] = hand; //add Hand to dictionary only

                    Console.WriteLine($"Hand created for Player {player.PlayerId} in Game {this.GameId}");
                }
            }
        }

        //alternate dealing tiles to players
        public void DealTiles(int tilesPerPlayer)
        {
            Console.WriteLine("Game DealTiles debug - dealing tiles");

            //prevent starting game with out players - check dictionary not db
            if (!PlayerHands.Any())
            {
                throw new InvalidOperationException("No players found in PlayerHands.");
            }

            List<int> playerIds = PlayerHands.Keys.ToList(); //extract playerId values from _playerHands dictionary
            int currentPlayerIndex = 0; //set index to first player initially
            int totalTilesToDeal = tilesPerPlayer * playerIds.Count;

            for (int i = 0; i < totalTilesToDeal; i++)
            {
                //alternate players to deal
                currentPlayerIndex = currentPlayerIndex % playerIds.Count;

                //get the current player (alternating through the players in the Hands list)
                int currentPlayerId = playerIds[currentPlayerIndex]; //index of current player

                //make sure Player associated with dictionary playerhand
                if (PlayerHands.TryGetValue(currentPlayerId, out Hand playerHand))
                {
                    //pass players hand to deal method
                    Deal.DealTile(playerHand.Player, this.GameId, PlayerHands);
                }

                //increment index
                currentPlayerIndex++;
            }

            //add next tile in TileDeck to center of board
            Tile tile = TileDeck.RemoveTile(0);
            GameBoard.PlaceTile(tile, 0);
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
            if (!PlayerHands.TryGetValue(playerId, out Hand? playerHand))
                if (playerHand == null)
            {
                //change to display a message to select starting player before picking up a tile
                throw new InvalidOperationException($"Player {playerId} does not have a hand in this game.");
            }

            Deal.DrawTile(playerHand.Player, this.GameId, PlayerHands);
            Score++;
        }

        //place Tile on GameBoard (simplified to horizontal exansion only)
        public PlacementStatus PlaceTile(int playerId, Tile tile, int position)
        {
            Console.WriteLine($"Game PlaceTile debug - Placing tile - Player ID: {playerId}, Tile ID: {tile?.Id}, Color: {tile?.Color}, position: {position}");
            
            //check if player has a hand
            if (!PlayerHands.TryGetValue(playerId, out Hand? playerHand))
            {
                Console.WriteLine($"Error: Player {playerId} does not have a hand.");
                return PlacementStatus.NoHand;
            }

            //Compare by Tile Id. Converting DTO creates new Tile instance. Comparing by object reference will never match Tile object in players hand
            int tileIndex = playerHand.HandTiles.FindIndex(t => t.Id == tile.Id);
            if (tileIndex < 0)
            {
                Console.WriteLine($"Error: Tile {tile.Id} not found in player {playerId}'s hand.");
                return PlacementStatus.TileNotFound;
            }

            //validate chosen placement on Game Board
            PlacementStatus validationResult = GameBoard.ValidatePlacement(this, playerId, tile, position);
            if (validationResult != PlacementStatus.Success)
            {
                return validationResult; // Return the failure reason
            }

            //remove tile from players hand and place on board
            playerHand.HandTiles.RemoveAt(tileIndex);
            GameBoard.PlaceTile(tile, position);

            Console.WriteLine($"Game PlaceTile debug - Tile placed successfully at position {position}");
            foreach (var placed in GameBoard.PlacedTiles)
            {
                Console.WriteLine($"Tile ID: {placed.Tile.Id}, Position: {placed.Position}");
            }

            return PlacementStatus.Success;
        }

        //Turn Handling
        public void NextTurn()
        {
            //find index of current player from dictionary
            List<int> playerIds = PlayerHands.Keys.ToList();
            int currentIndex = playerIds.IndexOf(CurrentPlayerId);

            //safety check for invalid CurrentPlayerId
            if (currentIndex == -1)
            {
                throw new InvalidOperationException("Current PlayerId invalid");
            }

            //get next player index
            int nextIndex = (currentIndex + 1) % playerIds.Count;

            //set iterations to 0 at start of while loop. used to prevent infinite loop
            int iterations = 0;
            //find next player with tiles (while loop chosen in anticipation of being able to play with more than 2 players in future)
            while (PlayerHands[playerIds[nextIndex]].HandTiles.Count == 0 && iterations < playerIds.Count)
            {
                nextIndex = (nextIndex + 1) % playerIds.Count; //move index to next player (moves back to current player if no other player has tiles)
                iterations++; //increment iterations to only loop the amount of times as there are players
            }

            //end game if all players hands have been checked and all found to have no tiles left
            if (PlayerHands[playerIds[nextIndex]].HandTiles.Count == 0)
            {
                Console.WriteLine("Game has ended");
                GameOver = true; //switch flag to true to signify end of game
                return; //game over. stop further execution.
            }

            //debug - check to make sure CurrentPlayerId is updating
            Console.WriteLine($"Game NextTurn debug - CurrentPlayerId before: {CurrentPlayerId}");
            CurrentPlayerId = playerIds[nextIndex];
            Console.WriteLine($"Game NextTurn debug - CurrentPlayerId after: {CurrentPlayerId}");
        }

        //ToString
        public override string ToString()
        {
            return $"Game ID: {GameId}, Colors: {NumberOfColors}, Tiles: {NumberOfTiles}";
        }
    }
}
