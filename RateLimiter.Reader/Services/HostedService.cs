namespace RateLimiter.Reader.Services;

public class HostedService : IHostedService
{
    private IReaderService _readerService;
    private CancellationTokenSource _watchTokenSource;

    public HostedService(IReaderService readerService)
    {
        _readerService = readerService;
        _watchTokenSource = new CancellationTokenSource();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Загружаем данные батчами
            await _readerService.LoadRateLimitsInBatchesAsync(cancellationToken);
            // Стартуем отслеживание изменений
            _ = Task.Run(() => _readerService.WatchRateLimitChangesAsync(_watchTokenSource.Token), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Batch loading was canceled."); // Ловить стоит, чтобы корректно обработать отмену
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _watchTokenSource.Cancel();
        return Task.CompletedTask;
    }
}