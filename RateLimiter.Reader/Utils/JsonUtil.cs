using System.Text.Json;
using RateLimiter.Reader.Models.Kafka;

namespace RateLimiter.Reader.Utils;

public class JsonUtil
{
    public static KafkaEvent? ParseKafkaEvent(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        try
        {
            var tempEvent = JsonSerializer.Deserialize<Dictionary<string, object>>(json, options);
        
            if (tempEvent != null && tempEvent.ContainsKey("user_id"))
            {
                if (long.TryParse(tempEvent["user_id"]?.ToString(), out long userId))
                {
                    return new KafkaEvent(userId, tempEvent["endpoint"].ToString());
                }
                else
                {
                    Console.WriteLine($"Error parsing user_id: {tempEvent["user_id"]}");
                    return null;
                }
            }
        
            Console.WriteLine("Missing user_id in the JSON");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return null;
        }
    }

}