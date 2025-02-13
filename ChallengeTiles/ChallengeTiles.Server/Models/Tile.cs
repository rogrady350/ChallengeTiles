namespace ChallengeTiles.Server.Models
{
    public class Tile
    {
        //constant for bucket url
        private const string BucketUrl = Constants.BucketUrl;

        //parameterless constructor if needed for EF
        public Tile() { }

        //constructor with paramaters
        public Tile(int id, int number, string color)
        {
            //set attributes
            Id = id;
            Number = number;
            Color = color;

            //set tile values
            SetTileImage();
        }

        //attributes, getters, setters (no setters for tiles, tiles immutible)
        public int Id { get; }
        public int Number { get; }
        public string Color { get; }
        public string TileImageUrl { get; set; }


        //Method to assign images to tiles. images will be stored on aws. verify path
        private void SetTileImage()
        {
            TileImageUrl = $"{BucketUrl}/{Number}{Color}";
        }

        //ToString
        public override string ToString()
        {
            return $"{Color}{Number}";
        }

        //Equals
        public override bool Equals(object? obj)
        {
            if (obj is not Tile otherTile) return false;

            return Id == otherTile.Id
                && Number == otherTile.Number
                && Color.Equals(otherTile.Color);
        }

        //Hashcode
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Number, Color);
        }
    }
}
