using Grpc.Core;
using RateLimiter.Writer.Models;
using RateLimiter.Writer.Services;

namespace RateLimiter.Writer.Controllers;

public class WriterServiceController : Writer.WriterBase
{
    private readonly IWriterService _writerService;
    private readonly IRateLimitToRateLimitReplyMapper _rateLimitMapper;

    public WriterServiceController(IWriterService writerService, IRateLimitToRateLimitReplyMapper rateLimitMapper)
    {
        _writerService = writerService;
        _rateLimitMapper = rateLimitMapper;
    }

    public override async Task<RateLimitReply> CreateRateLimit(CreateRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = await _writerService.CreateRateLimit(request, context.CancellationToken);
        return _rateLimitMapper.MapToRateLimitReply(rateLimit);
    }

    public override async Task<RateLimitReply> GetRateLimit(GetRateLimitByRouteRequest request, ServerCallContext context)
    {
        var rateLimit = await _writerService.GetRateLimit(request, context.CancellationToken);
        return _rateLimitMapper.MapToRateLimitReply(rateLimit);
    }

    public override async Task<RateLimitReply> UpdateRateLimit(UpdateRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = await _writerService.UpdateRateLimit(request, context.CancellationToken);
        return _rateLimitMapper.MapToRateLimitReply(rateLimit);
    }

    public override async Task<RateLimitReply> DeleteRateLimitByRoute(DeleteRateLimitByRouteRequest request, ServerCallContext context)
    {
        var rateLimit = await _writerService.DeleteRateLimit(request, context.CancellationToken);
        return _rateLimitMapper.MapToRateLimitReply(rateLimit);
    }
}