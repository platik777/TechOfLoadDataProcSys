using StackExchange.Redis;

namespace UserService.Services.Redis;

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
}
