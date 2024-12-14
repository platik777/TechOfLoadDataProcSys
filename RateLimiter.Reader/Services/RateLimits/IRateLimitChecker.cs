using RateLimiter.Reader.Models.Kafka;

namespace RateLimiter.Reader.Services.RateLimits;

public interface IRateLimitChecker
{
    Task<bool> CheckRateLimitAsync(KafkaEvent kafkaEvent);
}