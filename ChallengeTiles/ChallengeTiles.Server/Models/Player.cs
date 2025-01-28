namespace ChallengeTiles.Server.Models
{
    public class Player
    {
        private static int idCounter = 1;

        //Constructor
        public Player(string name, Hand hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand), "Hand cannot be null");
            if (name == null) throw new ArgumentNullException(nameof(name), "Hand cannot be null");

            Id = GenerateId();
            Name = name;
            Hand = hand;
        }

        //auto generate player id
        private static int GenerateId()
        {
            return idCounter++;
        }

        //attributres, getters, setters
        public int Id { get; }
        public string Name { get; }
        public Hand Hand { get; }

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
