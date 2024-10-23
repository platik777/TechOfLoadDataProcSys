using RateLimiter.Writer.Models.Entities;

namespace RateLimiter.Writer.Models;

public interface IRateLimitToRateLimitEntityMapper
{
    RateLimitEntity MapToRateLimitEntity(RateLimit rateLimit);
}