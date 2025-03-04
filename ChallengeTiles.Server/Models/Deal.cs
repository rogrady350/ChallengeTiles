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

        //deal tile to player
        public void DealTile(Player player, int gameId)
        {
            //avoid dealing from empty deck.
            if (TileDeck.GetTileCount() == 0)
            {
                throw new InvalidOperationException("No more tiles available to deal.");
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
