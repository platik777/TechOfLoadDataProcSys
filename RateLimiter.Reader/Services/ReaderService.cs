using System.Collections.Concurrent;
using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;

namespace RateLimiter.Reader.Services;

public class ReaderService : IReaderService
{
    private readonly IReaderRepository _databaseRepository;
    private readonly ConcurrentDictionary<string, RateLimit> _rateLimits;

    public ReaderService(IReaderRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
        _rateLimits = new ConcurrentDictionary<string, RateLimit>();
    }

    public async Task LoadRateLimitsInBatchesAsync(CancellationToken cancellationToken, int batchSize = 1000)
    {
        var rateLimits = _databaseRepository.GetRateLimitsBatchAsync(batchSize);
        
        await foreach (var rateLimit in rateLimits.WithCancellation(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            _rateLimits.TryAdd(rateLimit.Route, rateLimit);
        }
    }

    public async Task WatchRateLimitChangesAsync(CancellationToken cancellationToken)
    {
        await foreach (var (operationType, rateLimit) in _databaseRepository.WatchRateLimitChangesAsync(cancellationToken))
        {
            switch (operationType)
            {
                case ChangeStreamOperationType.Update:
                    if (_rateLimits.TryGetValue(rateLimit.Route, out var currentValue))
                        _rateLimits.TryUpdate(rateLimit.Route, rateLimit, currentValue);
                    break;
                case ChangeStreamOperationType.Delete:
                    _rateLimits.TryRemove(rateLimit.Route, out _);
                    break;
            }
        }
    }
    
    public IEnumerable<RateLimit> GetAllRateLimits()
    {
        return _rateLimits.Values;
    }
}