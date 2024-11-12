using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Models;

public interface IRateLimitToRateLimitReplyMapper
{
    GetRateLimitResponse MapToGetRateLimitResponse(RateLimit rateLimit);
    List<GetRateLimitResponse> MapToGetRateLimitResponseList(List<RateLimit> rateLimits);
    RateLimitsReply MapToRateLimitReply(List<RateLimit> rateLimits);
}