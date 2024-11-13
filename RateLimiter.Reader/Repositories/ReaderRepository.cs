using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;

namespace RateLimiter.Reader.Repositories;

public class ReaderRepository : IReaderRepository
{
    private readonly IMongoCollection<RateLimitEntity> _rateLimitsCollection;
    private readonly IRateLimitEntityToRateLimitMapper _rateLimitMapper;

    public ReaderRepository(IMongoDatabase database, IRateLimitEntityToRateLimitMapper rateLimitMapper)
    {
        _rateLimitsCollection = database.GetCollection<RateLimitEntity>("rate_limits");
        _rateLimitMapper = rateLimitMapper;
    }
    
    public async Task<List<RateLimit>> GetRateLimitsBatchAsync(int skip, int limit)
    {
        var rateLimitEntities = await _rateLimitsCollection
            .Find(_ => true)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();

        return rateLimitEntities.ConvertAll(entity => _rateLimitMapper.MapToRateLimit(entity));
    }
    
    public IChangeStreamCursor<ChangeStreamDocument<RateLimitEntity>> WatchRateLimitChanges()
    {
        return _rateLimitsCollection.Watch();
    }
    
    public RateLimit MapChangeToRateLimit(ChangeStreamDocument<RateLimitEntity> change)
    {
        return _rateLimitMapper.MapToRateLimit(change.FullDocument);
    }
}