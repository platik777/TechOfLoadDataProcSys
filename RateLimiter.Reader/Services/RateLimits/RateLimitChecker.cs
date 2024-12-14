using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Kafka;
using RateLimiter.Reader.Services.Redis;

namespace RateLimiter.Reader.Services.RateLimits;

public class RateLimitChecker : IRateLimitChecker
{
    private readonly TimeSpan _window;
    private readonly IRedisService _redisService;
    private readonly IReaderService _readerService;
    private readonly string _redisKeyPrefix = "rate_limit:";

    public RateLimitChecker(IRedisService redisService, IReaderService readerService)
    {
        _window = TimeSpan.FromMinutes(1);
        _redisService = redisService;
        _readerService = readerService;
    }
    
    public async Task<bool> CheckRateLimitAsync(KafkaEvent kafkaEvent)
    {
        var now = DateTime.UtcNow;
        var key = $"{_redisKeyPrefix}{kafkaEvent.UserId}:{kafkaEvent.Endpoint}";
        var timestamp = now.ToString("yyyyMMddHHmmss");

        if (await _redisService.IsRequestBlockedAsync(key))
        {
            return false;
        }

        await _redisService.AddRequestTimestampAsync(key, timestamp);

        var cutoffTimestamp = (now - _window).ToString("yyyyMMddHHmmss");
        await _redisService.CleanUpOldRequestsAsync(key, cutoffTimestamp);

        var requestCount = await _redisService.GetRequestCountAsync(key);
        if (requestCount >= FindRateLimit(kafkaEvent.Endpoint)!.RequestsPerMinute)
        {
            await _redisService.SetExceededFlagAsync(key);
            return false;
        }

        return true;
    }

    private RateLimit? FindRateLimit(string endpoint)
    {
        foreach (var value in _readerService.GetAllRateLimits())
        {
            if (value.Route == endpoint)
            {
                return value;
            }
        }

        return null;
    }
}