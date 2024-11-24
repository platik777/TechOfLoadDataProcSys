using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Repositories;

public interface ILocalReaderRepository
{
    IEnumerable<RateLimit> GetAllRateLimits();
    bool AddRateLimit(RateLimit rateLimit);
    bool UpdateRateLimit(RateLimit rateLimit);
    bool RemoveRateLimit(string route);
}