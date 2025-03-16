namespace ChallengeTiles.Server.Models
{
    public class Deal
    {
        //constructor (prepares deck for dealing)
        public Deal(TileDeck tileDeck)
        {
            this.TileDeck = tileDeck;
            PrepareDeck();
        }

        //attributes/getters
        public TileDeck TileDeck { get; }

        //create stack of tiles available for play
        public void PrepareDeck()
        {
            TileDeck.CreateTileDeck();
            TileDeck.ShuffleTiles();
        }

        //deal tile to player - should be adding to both db and dictionary
        public void DealTile(Player player, int gameId, Dictionary<int, Hand> playerHands)
        {
            //avoid dealing from empty deck.
            if (TileDeck.GetTileCount() == 0)
            {
                throw new InvalidOperationException("No more tiles available to deal.");
            }

            //retrieve hand from dictionary (dictionary modified during game play, database only holds initial Hand data)
            if (!playerHands.TryGetValue(player.PlayerId, out Hand? playerHand) || playerHand == null)
            {
                throw new InvalidOperationException($"Player {player.PlayerId} does not have a hand for Game {gameId}.");
            }

            Tile tile = TileDeck.RemoveTile(0); //remove Tile from deck
            playerHand.AddTile(tile);           //add to dictionary used for Hand during game play
        }

        //player draws a tile from the deck when they have no playable tiles
        public void DrawTile(Player player, int gameId)
        {
            //avoid drawing from an empty deck
            if (TileDeck.GetTileCount() == 0)
            {
                throw new InvalidOperationException("No more tiles available to draw.");
            }

            //find the player's hand for the given game
            Hand? playerHand = player.Hands.FirstOrDefault(h => h.GameId == gameId);
            if (playerHand == null)
            {
                throw new InvalidOperationException($"Player does not have a hand for Game {gameId}.");
            }

            //remove tile from deck and add to player's hand
            Tile tile = TileDeck.RemoveTile(0);
            playerHand.AddTile(tile);
        }
    }
}
