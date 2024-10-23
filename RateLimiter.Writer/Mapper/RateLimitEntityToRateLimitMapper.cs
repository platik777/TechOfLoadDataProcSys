using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Writer.Mapper;

[Mapper]
public partial class RateLimitEntityToRateLimitMapper : IRateLimitEntityToRateLimitMapper
{
    public partial RateLimit MapToRateLimit(RateLimitEntity rateLimitEntity);
}