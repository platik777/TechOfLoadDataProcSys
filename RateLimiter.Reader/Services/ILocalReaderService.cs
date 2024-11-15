using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Services;

public interface ILocalReaderService
{
    IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync();
    Task AddRateLimitAsync(RateLimit rateLimit);
    Task UpdateRateLimitAsync(RateLimit rateLimit);
    Task RemoveRateLimitAsync(string route);
}