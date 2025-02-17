namespace ChallengeTiles.Server.Models.Authentication
{
    public class User
        //class represents user
    {
        public int UserId { get; set; } 
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
}
