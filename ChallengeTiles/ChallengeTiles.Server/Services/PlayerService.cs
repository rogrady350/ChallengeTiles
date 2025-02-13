using ChallengeTiles.Server.Data;
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

        public async Task<Player> GetPlayerByIdAsync(string playerID)
        {
            return await _playerRepository.GetPlayerByIdAsync(playerID);
        }
    }
}
