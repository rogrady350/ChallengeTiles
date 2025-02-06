namespace ChallengeTiles.Server.Models
{
    public class Player
    {
        //Constructor for guest player (player data not stored in db)
        public Player(Hand hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand), "Hand cannot be null");

            Id = "guest-" + Guid.NewGuid().ToString(); //unique session-based ID
            Name = "Guest";
            Hand = hand;
        }

        //Constructor for player with account
        public Player(string name, Hand hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand), "Hand cannot be null");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");

            Id = Guid.NewGuid().ToString(); //unique persistent ID
            Name = name;
            Hand = hand;
        }

        //attributres, getters, setters
        public string Id { get; private set; } //unique player id
        public string Name { get; private set; }
        public Hand Hand { get; private set; }

        //ToString
        public override string ToString()
        {
            return $"PlayerId:{Id}, Name:{Name}";
        }

        //Equals (based on unique player id)
        public override bool Equals(object? obj)
        {
            if (obj is Player otherPlayer)
            {
                return this.Id == otherPlayer.Id;
            }

            return false;
        }

        //Hashcode (based on unique player id)
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
