using Grpc.Core;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Services;

namespace RateLimiter.Reader.Controllers;

public class ReaderServiceController : ReaderService.ReaderServiceBase
{
    private readonly IReaderService _readerService;
    private readonly IRateLimitToRateLimitReplyMapper _rateLimitMapper;

    public ReaderServiceController(IReaderService readerService, IRateLimitToRateLimitReplyMapper rateLimitMapper)
    {
        _readerService = readerService;
        _rateLimitMapper = rateLimitMapper;
    }

    public override Task<RateLimitsReply> GetRateLimits(GetRateLimitsRequest request, ServerCallContext context)
    {
        var rateLimits = _readerService.GetAllRateLimits().ToList();
        var rateLimitsReply = _rateLimitMapper.MapToRateLimitReply(rateLimits);
        
        return Task.FromResult(rateLimitsReply);
    }

}