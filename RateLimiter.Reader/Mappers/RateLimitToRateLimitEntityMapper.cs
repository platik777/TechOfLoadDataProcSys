using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Reader.Mappers;

[Mapper]
public partial class RateLimitToRateLimitEntityMapper : IRateLimitToRateLimitEntityMapper
{
    public partial RateLimitEntity MapToRateLimitEntity(RateLimit rateLimit);
}