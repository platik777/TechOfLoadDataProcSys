namespace UserService.Services.Redis;

public interface IRedisService
{
    Task<bool> IsRequestBlockedAsync(string key);
}