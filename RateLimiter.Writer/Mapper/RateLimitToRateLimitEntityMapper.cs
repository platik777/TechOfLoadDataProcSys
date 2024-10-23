using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Writer.Mapper;

[Mapper]
public partial class RateLimitToRateLimitEntityMapper : IRateLimitToRateLimitEntityMapper
{
    public partial RateLimitEntity MapToRateLimitEntity(RateLimit rateLimit);
}