namespace ChallengeTiles.Server.Models
{
    public class Player
    {
        private static int idCounter = 1;

        //default constructor
        public Player(Hand hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand), "Hand cannot be null");

            this.Name = "Player";
            this.Id = GenerateId();
            this.Hand = hand;
        }

        //Constructor with name entered by player
        public Player(string name, Hand hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand), "Hand cannot be null");
            if (name == null) throw new ArgumentNullException(nameof(name), "Hand cannot be null");

            this.Id = GenerateId();
            this.Name = name;
            this.Hand = hand;
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
