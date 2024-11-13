using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Repositories;

public interface ILocalReaderRepository
{
    IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync();
    Task AddRateLimitAsync(RateLimit rateLimit);
    Task UpdateRateLimitAsync(RateLimit rateLimit);
    Task RemoveRateLimitAsync(string route);
}