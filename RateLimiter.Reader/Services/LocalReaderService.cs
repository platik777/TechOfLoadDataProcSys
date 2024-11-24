using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;

namespace RateLimiter.Reader.Services;

public class LocalReaderService : ILocalReaderService
{
    private readonly ILocalReaderRepository _localReaderRepository;

    public LocalReaderService(ILocalReaderRepository localReaderRepository)
    {
        _localReaderRepository = localReaderRepository;
    }

    public IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync()
    {
        return _localReaderRepository.GetAllRateLimits();
    }

    
    public async Task AddRateLimitAsync(RateLimit rateLimit)
    {
        await _localReaderRepository.AddRateLimit(rateLimit);
    }

    public async Task UpdateRateLimitAsync(RateLimit rateLimit)
    {
        await _localReaderRepository.UpdateRateLimit(rateLimit);
    }
    
    public Task RemoveRateLimitAsync(string route)
    {
        return _localReaderRepository.RemoveRateLimit(route);
    }
}