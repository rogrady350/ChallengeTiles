using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;

namespace ChallengeTiles.Server.Services
{
    //Class purpose: holds game play logic
    public class GameService
    {
        private readonly GameRepository _gameRepository;

        public GameService(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Game StartNewGame(int numberOfColors, int numberOfTiles)
        {
            var game = new Game(numberOfColors, numberOfTiles);
            _gameRepository.CreateGame(game, numberOfColors, numberOfTiles); //save game to DB
            return game;
        }

        public Game GetGameById(int gameId)
        {
            var game = _gameRepository.GetGameById(gameId);
            return game;
        }
    }
}
