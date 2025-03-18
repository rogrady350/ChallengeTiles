using ChallengeTiles.Server.Models.GameLogic;

namespace ChallengeTiles.Server.Helpers
{
    public class GameStateManager
    {
        //dictionary for in game management
        private readonly Dictionary<int, Game> _activeGames = new Dictionary<int, Game>();

        public Game? GetGame(int gameId)
        {
            _activeGames.TryGetValue(gameId, out var game);
            return game;
        }

        public void StoreGame(int gameId, Game game)
        {
            _activeGames[gameId] = game;
        }

        public bool GameExists(int gameId)
        {
            return _activeGames.ContainsKey(gameId);
        }
    }
}
