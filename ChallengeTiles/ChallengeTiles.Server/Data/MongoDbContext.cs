using ChallengeTiles.Server.Models;
using ChallengeTiles.Server.Models.GamePlay;
using MongoDB.Driver;

namespace ChallengeTiles.Server.Data
{
    public class MongoDbContext
    {
        //Mongo not currently being used. Class not finalized
        //MongoDb database interface. Implemented by MongoDatabase inside MongoDB driver
        private readonly IMongoDatabase _database;

        /*constructor, creates connection to db
          MongoDB does not use EF Core, no built in dependency injection
          Uses MongoDbSettings for manually storing ConnectionString and DatabaseName*/
        public MongoDbContext(string connectionString)
        {
            var client = new MongoClient(connectionString); //connection to MongoDB server
            _database = client.GetDatabase(Environment.GetEnvironmentVariable("MONGO_DB") ?? "WildTiles");
        }

        //MongoCollections
        public IMongoCollection<Player> Players => _database.GetCollection<Player>("Players");
        public IMongoCollection<Game> Games => _database.GetCollection<Game>("Games");
        public IMongoCollection<Tile> Tiles => _database.GetCollection<Tile>("Tiles");
    }
}
