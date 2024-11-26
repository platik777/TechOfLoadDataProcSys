using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Services;

public interface IReaderService
{
    Task WatchRateLimitChangesAsync(CancellationToken cancellationToken);
    Task LoadRateLimitsInBatchesAsync(CancellationToken cancellationToken, int batchSize = 1000);
    IEnumerable<RateLimit> GetAllRateLimits();
}