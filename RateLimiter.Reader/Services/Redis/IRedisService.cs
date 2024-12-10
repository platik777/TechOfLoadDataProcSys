namespace RateLimiter.Reader.Services.Redis;

public interface IRedisService
{
    Task<bool> IsRequestBlockedAsync(string key);
    Task<bool> SetExceededFlagAsync(string key);
    Task<long> GetRequestCountAsync(string key);
    Task AddRequestTimestampAsync(string key, string timestamp);
    Task CleanUpOldRequestsAsync(string key, string timestamp);
}