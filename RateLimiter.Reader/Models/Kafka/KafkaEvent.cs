using System.Text.Json.Serialization;

namespace RateLimiter.Reader.Models.Kafka;

public record KafkaEvent
{
    public KafkaEvent(long userId, string endpoint)
    {
        UserId = userId;
        Endpoint = endpoint;
    }
    
    [JsonPropertyName("user_id")]
    public long UserId { get; init; }

    [JsonPropertyName("endpoint")]
    public string Endpoint { get; init; }
}