using RateLimiter.Reader.Models.Entities;

namespace RateLimiter.Reader.Models;

public interface IRateLimitToRateLimitEntityMapper
{
    RateLimitEntity MapToRateLimitEntity(RateLimit rateLimit);
}