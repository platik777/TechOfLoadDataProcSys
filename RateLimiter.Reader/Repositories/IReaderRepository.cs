using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;

namespace RateLimiter.Reader.Repositories;

public interface IReaderRepository
{
    IAsyncEnumerable<RateLimit> GetRateLimitsBatchAsync(int batchSize);

    IAsyncEnumerable<(ChangeStreamOperationType OperationType, RateLimit RateLimit)> WatchRateLimitChangesAsync(
        CancellationToken cancellationToken);
}