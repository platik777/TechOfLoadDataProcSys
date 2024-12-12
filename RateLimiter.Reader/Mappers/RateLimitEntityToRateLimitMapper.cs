using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Reader.Mappers;

[Mapper]
public partial class RateLimitEntityToRateLimitMapper : IRateLimitEntityToRateLimitMapper
{
    public partial RateLimit MapToRateLimit(RateLimitEntity rateLimitEntity);
}