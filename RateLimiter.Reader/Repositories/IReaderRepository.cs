using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;

namespace RateLimiter.Reader.Repositories;

public interface IReaderRepository
{
    IAsyncEnumerable<(string id, RateLimit rateLimit)> GetRateLimitsBatchAsync(int batchSize);

    IAsyncEnumerable<(ChangeStreamOperationType OperationType, string? id, RateLimit? RateLimit)> WatchRateLimitChangesAsync(
        CancellationToken cancellationToken);
}