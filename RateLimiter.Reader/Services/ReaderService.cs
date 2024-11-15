using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;

namespace RateLimiter.Reader.Services;

public class ReaderService : IReaderService
{
    private readonly IReaderRepository _databaseRepository;
    private readonly ILocalReaderService _localReaderService;
    private int _offset;

    public ReaderService(
        IReaderRepository databaseRepository,
        ILocalReaderService localReaderService)
    {
        _databaseRepository = databaseRepository;
        _localReaderService = localReaderService;
        _offset = 0;
    }

    public async Task LoadRateLimitsInBatchesAsync(CancellationToken cancellationToken, int batchSize = 1000)
    {
        List<RateLimit> batch;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            batch = await _databaseRepository.GetRateLimitsBatchAsync(_offset, batchSize);
            foreach (var rateLimit in batch)
            {
                await _localReaderService.AddRateLimitAsync(rateLimit);
            }

            _offset += batch.Count;
        } while (batch.Count == batchSize);
    }

    public async Task WatchRateLimitChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var cursor = await _databaseRepository.WatchRateLimitChanges();

            while (await cursor.MoveNextAsync(cancellationToken)) 
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                foreach (var change in cursor.Current) 
                {
                    var updatedRateLimit = _databaseRepository.MapChangeToRateLimit(change);
                    await _localReaderService.UpdateRateLimitAsync(updatedRateLimit);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error watching rate limit changes: {ex.Message}");
            throw;
        }
    }
}