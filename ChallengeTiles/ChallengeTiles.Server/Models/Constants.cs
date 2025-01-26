namespace ChallengeTiles.Server.Models
{
    public class Constants
    {
        //aws s3 bucket url for tile images
        public const string BucketUrl = "https://wild-tile-images.s3.us-east-2.amazonaws.com";

        //list of all colors that can be played
        public static readonly List<string> availableColors = new List<string> { "red", "blue", "green", "yellow" };

        //constants for min and max values of tiles
        public const int TileMin = 1;
        public const int TileMax = 13;
    }
}
