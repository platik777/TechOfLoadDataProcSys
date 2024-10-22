using RateLimiter.Writer.Models;

namespace RateLimiter.Writer.Services;

public interface IWriterService
{
    Task<RateLimit> CreateRateLimit(CreateRateLimitRequest request, CancellationToken cancellationToken);
    Task<RateLimit> GetRateLimit(GetRateLimitByRouteRequest request, CancellationToken cancellationToken);
    Task<RateLimit> UpdateRateLimit(UpdateRateLimitRequest request, CancellationToken cancellationToken);
    Task<RateLimit> DeleteRateLimit(DeleteRateLimitByRouteRequest request, CancellationToken cancellationToken);
}