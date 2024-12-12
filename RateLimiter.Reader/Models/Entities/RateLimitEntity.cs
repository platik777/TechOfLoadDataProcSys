using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Reader.Models.Entities;

public class RateLimitEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Route")]
    public string Route { get; set; }

    [BsonElement("RequestsPerMinute")]
    public int RequestsPerMinute { get; set; }

    public RateLimitEntity(string route, int requestsPerMinute)
    {
        Route = route;
        RequestsPerMinute = requestsPerMinute;
    }
}