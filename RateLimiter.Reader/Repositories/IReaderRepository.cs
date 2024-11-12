using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Repositories;

public interface IReaderRepository
{
    Task<List<RateLimit>> GetByAllAsync();
}