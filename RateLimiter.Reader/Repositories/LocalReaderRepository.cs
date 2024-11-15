using System.Collections.Concurrent;
using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Repositories;

public class LocalReaderRepository : ILocalReaderRepository
{
    private readonly ConcurrentBag<RateLimit> _rateLimits;

    public LocalReaderRepository()
    {
        _rateLimits = new ConcurrentBag<RateLimit>();
    }

    public Task AddRateLimitAsync(RateLimit rateLimit)
    {
        if (!_rateLimits.Any(r => r.Route == rateLimit.Route))
        {
            _rateLimits.Add(rateLimit);
        }
        return Task.CompletedTask;
    }
    
    public Task UpdateRateLimitAsync(RateLimit rateLimit)
    {
        var updatedRateLimits = _rateLimits
            .Where(r => r.Route != rateLimit.Route)
            .ToList();
        updatedRateLimits.Add(rateLimit);
        
        _rateLimits.Clear();
        
        foreach (var updatedRateLimit in updatedRateLimits)
        {
            _rateLimits.Add(updatedRateLimit);
        }

        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync()
    {
        foreach (var rateLimit in _rateLimits)
        {
            yield return rateLimit;
            await Task.Yield(); 
        }
    }

    public Task RemoveRateLimitAsync(string route)
    {
        var remainingRateLimits = _rateLimits
            .Where(r => r.Route != route)
            .ToList();
        
        _rateLimits.Clear();
        
        foreach (var remainingRateLimit in remainingRateLimits)
        {
            _rateLimits.Add(remainingRateLimit);
        }

        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<RateLimit> GetRateLimitChangesAsync()
    {
        await foreach (var rateLimit in GetAllRateLimitsAsync())
        {
            yield return rateLimit;
        }
    }
}