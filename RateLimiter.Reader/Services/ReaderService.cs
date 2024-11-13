using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;

namespace RateLimiter.Reader.Services;

public class ReaderService : IReaderService, IDisposable
{
    private readonly ILocalReaderRepository _localReaderRepository;
    private SemaphoreSlim _updateSemaphore;
    private int _requestsPerMinute;

    public ReaderService(ILocalReaderRepository localReaderRepository, int requestsPerMinute)
    {
        _localReaderRepository = localReaderRepository;
        _requestsPerMinute = requestsPerMinute;
        
        _updateSemaphore = new SemaphoreSlim(_requestsPerMinute, _requestsPerMinute);
    }

    public IAsyncEnumerable<RateLimit> GetAllRateLimitsAsync()
    {
        return _localReaderRepository.GetAllRateLimitsAsync();
    }

    
    public async Task AddRateLimitAsync(RateLimit rateLimit)
    {
        // Ждём освобождения слота в семафоре перед обновлением, чтобы соблюдать RPM
        await _updateSemaphore.WaitAsync();
        
        await _localReaderRepository.AddRateLimitAsync(rateLimit);
    }

    public async Task UpdateRateLimitAsync(RateLimit rateLimit)
    {
        await _updateSemaphore.WaitAsync();
        
        await _localReaderRepository.UpdateRateLimitAsync(rateLimit);
    }
    
    public Task RemoveRateLimitAsync(string route)
    {
        return _localReaderRepository.RemoveRateLimitAsync(route);
    }

    // Метод для установки нового значения RPM
    public void SetRequestsPerMinute(int newRpm)
    {
        if (newRpm == _requestsPerMinute) return;

        _requestsPerMinute = newRpm;
        
        _updateSemaphore = new SemaphoreSlim(_requestsPerMinute, _requestsPerMinute);
    }

    public void Dispose()
    {
        _updateSemaphore?.Dispose();
    }
}