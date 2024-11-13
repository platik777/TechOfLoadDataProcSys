using System.Collections.Concurrent;
using RateLimiter.Reader.Models;

namespace RateLimiter.Reader.Repositories;

public class LocalReaderRepository : ILocalReaderRepository
{
    private readonly ConcurrentDictionary<string, RateLimit> _rateLimits;

    public LocalReaderRepository()
    {
        _rateLimits = new ConcurrentDictionary<string, RateLimit>();
    }
    
    public Task AddRateLimitAsync(RateLimit rateLimit)
    {
        _rateLimits.TryAdd(rateLimit.Route, rateLimit);
        return Task.CompletedTask;
    }

    
    public Task UpdateRateLimitAsync(RateLimit rateLimit)
    {
        if (_rateLimits.TryGetValue(rateLimit.Route, out var currentValue))
        {
            _rateLimits.TryUpdate(rateLimit.Route, rateLimit, currentValue);
        }
        return Task.CompletedTask;
    }

    
    public async IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync()
    {
        foreach (var rateLimit in _rateLimits.Values)
        {
            yield return rateLimit;
        }
        await Task.CompletedTask;
    }
    
    public Task RemoveRateLimitAsync(string route)
    {
        _rateLimits.TryRemove(route, out _);
        return Task.CompletedTask;
    }
}