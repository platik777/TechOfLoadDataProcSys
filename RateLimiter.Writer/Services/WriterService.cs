using RateLimiter.Writer.Models;
using RateLimiter.Writer.Repository;

namespace RateLimiter.Writer.Services;

public class WriterService : IWriterService
{
    private readonly IWriterRepository _writerRepository;

    public WriterService(IWriterRepository writerRepository)
    {
        _writerRepository = writerRepository;
    }

    public async Task<RateLimit> CreateRateLimit(CreateRateLimitRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<RateLimit> GetRateLimit(GetRateLimitByRouteRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<RateLimit> UpdateRateLimit(UpdateRateLimitRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<RateLimit> DeleteRateLimit(DeleteRateLimitByRouteRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}