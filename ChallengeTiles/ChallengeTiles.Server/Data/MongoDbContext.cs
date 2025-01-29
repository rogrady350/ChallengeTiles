using ChallengeTiles.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeTiles.Server.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(Environment.GetEnvironmentVariable("MONGO_DB") ?? "WildTiles");
        }

        public IMongoCollection<Player> Players => _database.GetCollection<Player>("Players");
        public IMongoCollection<Game> Games => _database.GetCollection<Game>("Games");
        public IMongoCollection<Tile> Tiles => _database.GetCollection<Tile>("Tiles");
    }
}
