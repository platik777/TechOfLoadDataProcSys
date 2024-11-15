using Grpc.Core;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Services;

namespace RateLimiter.Reader.Controllers;

public class ReaderServiceController : ReaderService.ReaderServiceBase
{
    private readonly ILocalReaderService _localReaderService;
    private readonly IRateLimitToRateLimitReplyMapper _rateLimitMapper;

    public ReaderServiceController(
        ILocalReaderService localReaderService,
        IRateLimitToRateLimitReplyMapper rateLimitMapper)
    {
        _localReaderService = localReaderService;
        _rateLimitMapper = rateLimitMapper;
    }

    public override async Task<RateLimitsReply> GetRateLimits(GetRateLimitsRequest request, ServerCallContext context)
    {
        var rateLimitsList = new List<RateLimit>();

        await foreach (var rateLimit in _localReaderService.GetAllRateLimitsAsync())
        {
            rateLimitsList.Add(rateLimit);
        }

        return _rateLimitMapper.MapToRateLimitReply(rateLimitsList);
    }

}