using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;

namespace RateLimiter.Writer.Repository;

public interface IWriterRepository
{
    Task<RateLimit> GetByRouteAsync(string id, CancellationToken cancellationToken);
    Task<RateLimit> CreateAsync(RateLimit rateLimit, CancellationToken cancellationToken);
    Task<RateLimit?> UpdateAsync(RateLimit rateLimit, CancellationToken cancellationToken);
    Task<RateLimit?> DeleteAsync(string id, CancellationToken cancellationToken);
}