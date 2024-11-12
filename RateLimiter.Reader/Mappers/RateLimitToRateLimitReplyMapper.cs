using RateLimiter.Reader.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Reader.Mappers;

[Mapper]
public partial class RateLimitToRateLimitReplyMapper : IRateLimitToRateLimitReplyMapper
{
    public partial GetRateLimitResponse MapToGetRateLimitResponse(RateLimit rateLimit);

    public partial List<GetRateLimitResponse> MapToGetRateLimitResponseList(List<RateLimit> rateLimits);

    public RateLimitsReply MapToRateLimitReply(List<RateLimit> rateLimits)
    {
        var rateLimitResponses = MapToGetRateLimitResponseList(rateLimits);
        return new RateLimitsReply { RateLimits = { rateLimitResponses } };
    }
}