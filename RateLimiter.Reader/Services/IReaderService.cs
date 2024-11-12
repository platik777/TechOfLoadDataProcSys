using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Services;

public interface IReaderService
{
    Task<List<RateLimit>> GetRateLimits(GetRateLimitsRequest request);
}