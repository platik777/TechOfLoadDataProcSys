using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;

namespace RateLimiter.Reader.Repositories;

public interface IReaderRepository
{
    Task<List<RateLimit>> GetRateLimitsBatchAsync(int skip, int limit);
    IChangeStreamCursor<ChangeStreamDocument<RateLimitEntity>> WatchRateLimitChanges();
    RateLimit MapChangeToRateLimit(ChangeStreamDocument<RateLimitEntity> change);
}