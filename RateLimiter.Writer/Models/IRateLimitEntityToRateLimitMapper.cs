using RateLimiter.Writer.Models.Entities;

namespace RateLimiter.Writer.Models;

public interface IRateLimitEntityToRateLimitMapper
{
    RateLimit MapToRateLimit(RateLimitEntity rateLimitEntity);
}