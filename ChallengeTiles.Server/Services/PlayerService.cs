using BCrypt.Net;
using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace ChallengeTiles.Server.Services
{
    //class purpose: Manages player profiles, game history, game statistics
    public class PlayerService
    {
        private readonly PlayerRepository _playerRepository;

        //constructor
        public PlayerService(PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        //hashing method. create now to already having in place when adding authentication
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //profile field validations
        public ServiceResponse<Player> ValidateInput(string username, string password, string email, string name)
        {
            var response = new ServiceResponse<Player>();

            //empty username
            if (string.IsNullOrWhiteSpace(username))
            {
                response.Success = false;
                response.Message = "Username cannot be empty";
                return response;
            }

            //taken username
            if (_playerRepository.GetPlayerByUsername(username) != null)
            {
                response.Success = false;
                response.Message = "Username already taken";
                return response;
            }

            //empty password
            if (string.IsNullOrWhiteSpace(password))
            {
                response.Success = false;
                response.Message = "Password cannot be empty";
                return response;
            }

            //empty email
            if (string.IsNullOrWhiteSpace(email))
            {
                response.Success = false;
                response.Message = "Email cannot be empty";
                return response;
            }

            //email format check
            if (!new EmailAddressAttribute().IsValid(email))
            {
                response.Success = false;
                response.Message = "Please enter a vaild email address";
                return response;
            }

            //already used email
            if(_playerRepository.GetPlayerByEmail(email) != null)
            {
                response.Success = false;
                response.Message = "Email is already in use";
                return response;
            }

            //empty name
            if (string.IsNullOrWhiteSpace(name))
            {
                response.Success = false;
                response.Message = "Name cannot be empty";
                return response;
            }

            //all validations passed
            response.Success = true;
            return response;
        }

        //create a new Player profile
        public ServiceResponse<Player> RegisterProfile(string username, string password, string email, string name)
        {
            //call method to validate repsonse
            ServiceResponse<Player> response = ValidateInput(username, password, email, name);
            
            //if validation fails return failure reponse
            if (!response.Success) return response;

            string hashedPassword = HashPassword(password);

            //add new player to database
            Player player = new Player(username, password, email, name);
            _playerRepository.CreateProfile(player);

            //responses for succesful profile creation
            response.Success = true;
            response.Data = player;
            response.Message = "Player registered sucessfully";

            return response;
        }

        //retrieve a single player (used for sorting by player)
       public Player GetPlayer(int playerId)
        {
            Player player = _playerRepository.GetPlayerById(playerId);
            return player;
        }

        //retrieve all players in db
        public IEnumerable<Player> GetAllPlayers()
        {
            IEnumerable<Player> playerList = _playerRepository.GetAllPlayers();
            return playerList;
        }

        //no options to implement UpdatePlayer() or DeletePlayer(). Fepository methods available for future use
    }
}
