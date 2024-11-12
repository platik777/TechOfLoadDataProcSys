using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Models;

public interface IRateLimitToRateLimitReplyMapper
{
    RateLimitsReply MapToRateLimitReply(List<RateLimit> user);
}