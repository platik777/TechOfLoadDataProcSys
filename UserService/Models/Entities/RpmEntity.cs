namespace UserService.Models.Entities;

public class RpmEntity : IRpmModel
{
    public RpmEntity(long userId, string endpoint, int rpm)
    {
        UserId = userId;
        Endpoint = endpoint;
        Rpm = rpm;
    }

    public long UserId { get; set; }
    public string Endpoint { get; set; }
    public int Rpm { get; set; }
}