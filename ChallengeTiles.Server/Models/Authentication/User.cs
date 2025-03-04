namespace ChallengeTiles.Server.Models.Authentication
{
    public class User
    //class represents user
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } //hashed, stored as bytes
        public string Email { get; set; }
    }
}
