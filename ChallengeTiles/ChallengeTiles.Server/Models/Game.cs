namespace ChallengeTiles.Server.Models
{
    public class Game
    {
        //constructor
        public Game(int Id, int NumberOfColors)
        {
            this.Id = Id;
        }

        //attributres, getters, setters
        public int Id { get; set; }

        public int NumberOfColors { get; set; }
    }
}
