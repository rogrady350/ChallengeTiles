using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeTiles.Server.Models
{
    public class Player
    {
        //default no arg constructor for EF
        public Player()
        {
            Hands = new List<Hand>();  //initialize collection - ensure EF core handles the Player - Hand relationship
        }

        //constructor for registered player with account (option to play as guest can be added later)
        public Player(string username, string password, string email, string name)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username), "Username cannot be null or empty");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");

            Username = username;
            Password = password;
            Email = email;
            Name = name;
            Hands = new List<Hand>();  //initialize collection
        }

        //attributres, getters, setters
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; private set; } //unique player id. auto incremented in db

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Name { get; private set; }

        //1:n relationship. Each player gets 1 Hand per game
        public List<Hand> Hands { get; private set; }

        //ToString
        public override string ToString()
        {
            return $"PlayerId:{PlayerId}, Name:{Name}";
        }

        //Equals (based on unique player id)
        public override bool Equals(object? obj)
        {
            if (obj is Player otherPlayer)
            {
                return this.PlayerId == otherPlayer.PlayerId;
            }

            return false;
        }

        //Hashcode (based on unique player id)
        public override int GetHashCode()
        {
            return PlayerId.GetHashCode();
        }
    }
}
