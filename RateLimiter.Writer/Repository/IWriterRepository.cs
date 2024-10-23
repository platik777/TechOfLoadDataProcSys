using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;

namespace RateLimiter.Writer.Repository;

public interface IWriterRepository
{
    Task<RateLimitEntity> GetByRouteAsync(string id);
    Task<RateLimitEntity> CreateAsync(RateLimit rateLimit);
    Task<RateLimitEntity?> UpdateAsync(RateLimit rateLimit);
    Task DeleteAsync(string id);
}