using ChallengeTiles.Server.Data;
using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;

namespace ChallengeTiles.Server.Services
{
    //Class purpose: holds game play logic
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly PlayerRepository _playerRepository;

        public GameService(GameRepository gameRepository, PlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
        }

        public Game StartNewGame(List<int> playerIds, int numberOfColors, int numberOfTiles)
        {
            //1. fetch player objects from DB by playerId
            var players = new List<Player>();

            foreach (var playerId in playerIds)
            {
                var player = _playerRepository.GetPlayerById(playerId);
                if (player != null)
                {
                    players.Add(player);
                }
            }

            //2. create a new Game
            var game = new Game(numberOfColors, numberOfTiles);

            //3. add Players and deal Hands
            game.AddPlayers(players, numberOfTiles, game.TileDeck);

            //4. deal the tiles (numberOfTiles is Tiles per hand. multiply by number of players playing)
            game.DealTiles(numberOfTiles * playerIds.Count);

            //5. save the Game to the database
            _gameRepository.AddGame(game);

            //6.update database with populated Hands
            _gameRepository.UpdateGameHands(game);

            //7.display message asking for starting player


            return game;
        }

        public Game GetGameById(int gameId)
        {
            var game = _gameRepository.GetGameById(gameId);
            return game;
        }
    }
}
