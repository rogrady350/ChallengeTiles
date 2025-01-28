namespace ChallengeTiles.Server.Models
{
    public class Deal
    {
        private readonly TileDeck _tileDeck;

        //constructor
        public Deal(TileDeck tileDeck)
        {
            this._tileDeck = tileDeck;
            PrepareDeck();
        }

        //create stack of tiles available for play
        public void PrepareDeck()
        {
            _tileDeck.CreateTileDeck();
            _tileDeck.ShuffleTiles();
        }

        //deal tile to player
        public void DealTile(Player player)
        {
            Tile tile = _tileDeck.RemoveTile(0);
            Hand hand = player.Hand;
        }
    }
}
