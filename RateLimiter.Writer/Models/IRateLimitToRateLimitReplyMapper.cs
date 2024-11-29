namespace RateLimiter.Writer.Models;

public interface IRateLimitToRateLimitReplyMapper
{
    RateLimitReply MapToRateLimitReply(RateLimit user);
}