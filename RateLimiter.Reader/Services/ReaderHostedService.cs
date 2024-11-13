using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;

namespace RateLimiter.Reader.Services;

public class ReaderHostedService : IHostedService
{
    private readonly IReaderRepository _databaseRepository;
    private readonly IReaderService _readerService;
    private CancellationTokenSource _watchTokenSource;
    private int _offset;

    public ReaderHostedService(
        IReaderRepository databaseRepository,
        IReaderService readerService)
    {
        _databaseRepository = databaseRepository;
        _readerService = readerService;
        _watchTokenSource = new CancellationTokenSource();
        _offset = 0;
    }


    public async Task StartHostedServiceAsync(CancellationToken cancellationToken)
    {
        try
        {
            await LoadRateLimitsInBatchesAsync(cancellationToken);
            _ = Task.Run(() => WatchRateLimitChangesAsync(_watchTokenSource.Token), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Batch loading was canceled."); // нужно ли ловить ??
        }
    }

    // Есть разные токены (от CancellationTokenSource и от ServerCallContext),
    // если метод будет использоваться контроллером grpc, то CancellationTokenSource можно убрать и тогда надо вызывать
    // + надо спросить, что лучше ThrowIfCancellationRequested - кидает ошибку или IsCancellationRequested - bool
    public Task StopHostedServiceAsync(CancellationToken cancellationToken)
    {
        _watchTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private async Task LoadRateLimitsInBatchesAsync(CancellationToken cancellationToken, int batchSize = 1000)
    {
        List<RateLimit> batch;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            batch = await _databaseRepository.GetRateLimitsBatchAsync(_offset, batchSize);
            foreach (var rateLimit in batch)
            {
                await _readerService.AddRateLimitAsync(rateLimit);
            }

            _offset += batch.Count;
        } while (batch.Count == batchSize);
    }

    // Метод для отслеживания изменений через WatchAsync и их применения к локальной памяти
    private async Task WatchRateLimitChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var cursor = _databaseRepository.WatchRateLimitChanges();

            foreach (var change in cursor.ToEnumerable())
            {
                if (cancellationToken.IsCancellationRequested) break;

                var updatedRateLimit = _databaseRepository.MapChangeToRateLimit(change);

                // Применение обновлений с учетом RPM
                await _readerService.UpdateRateLimitAsync(updatedRateLimit);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error watching rate limit changes.", ex);
        }
    }
}