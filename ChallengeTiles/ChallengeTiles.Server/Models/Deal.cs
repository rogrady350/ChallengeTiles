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

        //create stack of tiles available for play
        public void PrepareDeck()
        {
            TileDeck.CreateTileDeck();
            TileDeck.ShuffleTiles();
        }

        //deal tile to player
        public void DealTile(Player player)
        {
            Tile tile = TileDeck.RemoveTile(0);
            player.Hand.AddTile(tile);
        }

        //attributes/getters
        public TileDeck TileDeck { get; }
    }
}
