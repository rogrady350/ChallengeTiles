namespace PasswordHash
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter password to hash: ");
            string password = Console.ReadLine();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            Console.WriteLine("Hashed password: " + hashedPassword);
        }
    }
}
