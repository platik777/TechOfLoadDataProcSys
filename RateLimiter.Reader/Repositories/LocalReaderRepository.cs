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
    
    public bool AddRateLimit(RateLimit rateLimit)
    {
        return _rateLimits.TryAdd(rateLimit.Route, rateLimit);
    }

    
    public bool UpdateRateLimit(RateLimit rateLimit)
    {
        if (_rateLimits.TryGetValue(rateLimit.Route, out var currentValue))
        {
           return _rateLimits.TryUpdate(rateLimit.Route, rateLimit, currentValue);
        }
        return false;
    }
    
    public IEnumerable<RateLimit> GetAllRateLimits()
    {
        return _rateLimits.Values;
    }
    
    public bool RemoveRateLimit(string route)
    {
        return _rateLimits.TryRemove(route, out _);
    }
}