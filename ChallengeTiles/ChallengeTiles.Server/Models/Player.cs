using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeTiles.Server.Models
{
    public class Player
    {
        //Default no arg constructor for EF
        public Player() { }

        //Constructor for guest player (no player profile in db) (NOT implemented in initial game development)
        public Player(Hand hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand), "Hand cannot be null");

            GuestId = "guest-" + Guid.NewGuid().ToString(); //unique session-based ID
            Name = "Guest";
            Hand = hand;
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
            Hand = new Hand(); //player is created with empty hand. populated during deal
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
        public Hand Hand { get; private set; }

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
