using RateLimiter.Writer.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Writer.Mapper;

[Mapper]
public partial class RateLimitToRateLimitReplyMapper : IRateLimitToRateLimitReplyMapper
{
    public partial RateLimitReply MapToRateLimitReply(RateLimit user);
}
