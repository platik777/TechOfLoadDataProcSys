using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;

namespace RateLimiter.Writer.Repository;

public interface IWriterRepository
{
    Task<RateLimitEntity> GetByRouteAsync(string id, CancellationToken cancellationToken);
    Task<RateLimitEntity> CreateAsync(RateLimit rateLimit, CancellationToken cancellationToken);
    Task<RateLimitEntity?> UpdateAsync(RateLimit rateLimit, CancellationToken cancellationToken);
    Task<RateLimitEntity> DeleteAsync(string id, CancellationToken cancellationToken);
}