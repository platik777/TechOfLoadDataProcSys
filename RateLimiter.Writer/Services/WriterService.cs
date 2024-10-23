using Grpc.Core;
using RateLimiter.Writer.Models;
using RateLimiter.Writer.Repository;

namespace RateLimiter.Writer.Services;

public class WriterService : IWriterService
{
    private readonly IWriterRepository _writerRepository;
    private readonly IRateLimitEntityToRateLimitMapper _rateLimitMapper;

    public WriterService(IWriterRepository rateLimitRepository, IRateLimitEntityToRateLimitMapper rateLimitMapper)
    {
        _writerRepository = rateLimitRepository;
        _rateLimitMapper = rateLimitMapper;
    }

    public async Task<RateLimit> CreateRateLimit(CreateRateLimitRequest request, CancellationToken cancellationToken)
    {
        var existingRateLimit = await _writerRepository.GetByRouteAsync(request.Route);
        if (existingRateLimit != null)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Rate limit already exists."));
        }

        var rateLimit = new RateLimit(request.Route, request.RequestsPerMinute);

        await _writerRepository.CreateAsync(rateLimit);

        return rateLimit;
    }

    public async Task<RateLimit> GetRateLimit(GetRateLimitByRouteRequest request, CancellationToken cancellationToken)
    {
        var rateLimit = await _writerRepository.GetByRouteAsync(request.Route);
        if (rateLimit == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Rate limit not found."));
        }

        return _rateLimitMapper.MapToRateLimit(rateLimit);
    }

    public async Task<RateLimit> UpdateRateLimit(UpdateRateLimitRequest request, CancellationToken cancellationToken)
    {
        var existingRateLimit = await _writerRepository.GetByRouteAsync(request.Route);
        if (existingRateLimit == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Rate limit not found."));
        }

        existingRateLimit.RequestsPerMinute = request.RequestsPerMinute;
        var rateLimit = new RateLimit(existingRateLimit.Route, existingRateLimit.RequestsPerMinute);
        await _writerRepository.UpdateAsync(rateLimit);

        return rateLimit;
    }

    public async Task DeleteRateLimit(DeleteRateLimitByRouteRequest request, CancellationToken cancellationToken)
    {
        await _writerRepository.DeleteAsync(request.Route);
    }
}