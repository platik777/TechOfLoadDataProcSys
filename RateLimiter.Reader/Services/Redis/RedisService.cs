using StackExchange.Redis;

namespace RateLimiter.Reader.Services.Redis;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;

    public RedisService(IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetValue<string>("Redis:Configuration");
        _redis = ConnectionMultiplexer.Connect(redisConfiguration);
    }

    public async Task<bool> IsRequestBlockedAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync($"{key}:exceeded") != RedisValue.Null;
    }

    public async Task<bool> SetExceededFlagAsync(string key)
    {
        var db = _redis.GetDatabase();
        Console.WriteLine($"Rate limit on {key}");
        return await db.StringSetAsync($"{key}:exceeded", "true", TimeSpan.FromMinutes(5));
    }

    public async Task<long> GetRequestCountAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.ListLengthAsync(key);
    }

    public async Task AddRequestTimestampAsync(string key, string timestamp)
    {
        var db = _redis.GetDatabase();
        await db.ListRightPushAsync(key, timestamp);
    }

    public async Task CleanUpOldRequestsAsync(string key, string cutoffTimestamp)
    {
        var db = _redis.GetDatabase();
        var currentListLength = await db.ListLengthAsync(key);
        for (long i = 0; i < currentListLength; i++)
        {
            var timestamp = await db.ListGetByIndexAsync(key, i);
            if (string.Compare(timestamp, cutoffTimestamp) < 0)
            {
                await db.ListRemoveAsync(key, timestamp);
            }
        }
    }
}
