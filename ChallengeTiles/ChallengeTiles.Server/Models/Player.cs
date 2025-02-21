using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeTiles.Server.Models
{
    public class Player
    {
        //Default no arg constructor for EF
        public Player()
        {
            Hands = new List<Hand>();  //initialize the collection
        }

        //Constructor for registered player with account
        public Player(string username, string passwordHash, string email, string name)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username), "Username cannot be null or empty");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException(nameof(passwordHash), "Password cannot be null or empty");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");

            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            Name = name;

            Hands = new List<Hand>();  //initialize the collection
        }

        //attributres, getters, setters
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; private set; } //unique player id. auto incremented in db

        [NotMapped] //exclude from db
        public string GuestId { get; private set; } //use GUID to generate session based id

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; } //store hashed passwords

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
