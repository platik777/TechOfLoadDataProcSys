namespace UserService.Models.Kafka;

public record Event(long UserId, string Endpoint);