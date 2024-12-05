using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using MongoDB.Driver;
using RateLimiter.Reader.CustomExceptions;
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
        
        await foreach (var (id, rateLimit) in rateLimits.WithCancellation(cancellationToken))
        {
            _rateLimits.TryAdd(id, rateLimit);
        }
    }

    public async Task WatchRateLimitChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var (operationType, id, rateLimit) in _databaseRepository.WatchRateLimitChangesAsync(cancellationToken))
            {
                if (id is null)
                {
                    throw new NullReferenceException();
                }
                switch (operationType)
                {
                    case ChangeStreamOperationType.Update:
                        _rateLimits.TryGetValue(id, out var currentValue);
                        if (rateLimit is not null && currentValue is not null)
                        {
                            _rateLimits.TryUpdate(id, rateLimit, currentValue);
                        }
                        break;
                    case ChangeStreamOperationType.Delete:
                        _rateLimits.TryRemove(id, out _);
                        break;
                    case ChangeStreamOperationType.Insert:

                        if (rateLimit != null) _rateLimits.TryAdd(id, rateLimit);
                        break;
                }
            }
            
        }
        catch (Exception ex) when (ex is MissingFullDocumentException or UnsupportedOperationTypeException)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        
    }
    
    public List<RateLimit> GetAllRateLimits()
    {
        return _rateLimits.Values.ToList();
    }
}