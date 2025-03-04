﻿using BCrypt.Net;
using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Helpers;
using ChallengeTiles.Server.Models;

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
            if (_playerRepository.GetPlayerByName(username) != null)
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

            if(_playerRepository.GetPlayerByName(email) != null)
            {
                response.Success = false;
                response.Message = "Email is already in use";
                return response;
            }

            //empty name
            if (string.IsNullOrWhiteSpace(username))
            {
                response.Success = false;
                response.Message = "Name cannot be empty";
                return response;
            }

            return response;
        }

        public ServiceResponse<Player> RegisterProfile(string username, string password, string email, string name)
        {
            //call method to validate repsonse
            ServiceResponse<Player> response = ValidateInput(username, password, email, name);
            
            //if validation fales return failure reponse
            if (!response.Success) return response;

            string hashedPassword = HashPassword(password);

            //add new player to database
            Player player = new Player(username, password, email, name);
            _playerRepository.CreateProfile(player);

            //responses for succesful profile creation
            response.Success = true;
            response.Data = player;
            response.Message = "Player registed sucessfully";

            return response;
        }

       
    }
}
