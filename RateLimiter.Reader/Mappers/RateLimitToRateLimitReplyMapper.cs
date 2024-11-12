using RateLimiter.Reader.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Reader.Mappers;

[Mapper]
public partial class RateLimitToRateLimitReplyMapper : IRateLimitToRateLimitReplyMapper
{
    public partial RateLimitsReply MapToRateLimitReply(List<RateLimit> user);
}