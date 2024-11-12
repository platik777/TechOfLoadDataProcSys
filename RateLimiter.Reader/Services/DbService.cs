using MongoDB.Driver;

namespace RateLimiter.Reader.Services;

public class DbService
{
    private readonly IMongoDatabase _database;

    public DbService(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
        _database = mongoClient.GetDatabase("mongo");
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}