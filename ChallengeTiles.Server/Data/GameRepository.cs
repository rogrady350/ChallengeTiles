﻿using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GameLogic;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public class GameRepository
    {
        //class handles CRUD operations for Game entities
        private readonly MysqlDbContext _dbContext;

        //constructor
        public GameRepository(MysqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //new record in game table
        public void CreateGame(Game game, int numberOfColors, int numberOfTiles, List<Player> players)
        {
            //get values for number of tiles and colors set in game
            game.NumberOfColors = numberOfColors;
            game.NumberOfTiles = numberOfTiles;

            _dbContext.Game.Add(game); //INSERT game record in db
            _dbContext.SaveChanges(); //save record to generate a GameId

            //Handle creating Hand record in here
            foreach (Player player in players)
            {
                //create hand for each Player and associate it with current Game
                Hand playerHand = new Hand
                {
                    GameId = game.GameId,
                    PlayerId = player.PlayerId
                };

                _dbContext.Hand.Add(playerHand); //INSERT hand record into db
            }

            _dbContext.SaveChanges(); //save changes with updated hands
        }

        //retrieve a game by id
        public Game GetGameById(int gameId)
        {
            var game = _dbContext.Game
                .AsNoTracking() //not modifying fetched entity, improve performance since no tracking needed for read operations.
                .Include(g => g.Hands)  //include the related hands
                .ThenInclude(h => h.Player)  //include the related players
                .FirstOrDefault(g => g.GameId == gameId);

            return game;
        }

        //retrieve all games (no current situations need all games but will have for possible future needs)
        public IEnumerable<Game> GetAllGames()
        {
            return _dbContext.Game.ToList();
        }

        //save game updates (no current situations need updating but will have for possible future needs)
        public void UpdateGame(Game game)
        {
            _dbContext.Game.Update(game); //EF updtates db record
            _dbContext.SaveChanges();
        }

        //save hand to db once populated
        public void UpdateGameHands(Game game)
        {
            _dbContext.Game.Update(game);
            foreach (Hand hand in game.Hands)
            {
                _dbContext.Hand.Update(hand); //update each player's hand
            }
            _dbContext.SaveChanges();
        }

        public void FinalizeGame(Game game, int finalScore)
        {
            game.Score = finalScore;
            _dbContext.Game.Update(game);
            _dbContext.SaveChanges();
        }
    }
}
