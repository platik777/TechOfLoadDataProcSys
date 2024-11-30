namespace KafkaEventsSender.Models.Entities;

public class RpmEntity
{
    public RpmEntity(string userId, string endpoint, int rpm)
    {
        UserId = userId;
        Endpoint = endpoint;
        Rpm = rpm;
    }

    public string UserId { get; set; }
    public string Endpoint { get; set; }
    public int Rpm { get; set; }
}