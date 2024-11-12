using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;

namespace RateLimiter.Reader.Models;

public interface IRateLimitEntityToRateLimitMapper
{
    RateLimit MapToRateLimit(RateLimitEntity rateLimitEntity);
}