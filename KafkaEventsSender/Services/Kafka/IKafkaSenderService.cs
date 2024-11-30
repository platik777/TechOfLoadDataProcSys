namespace KafkaEventsSender.Services.Kafka;

public interface IKafkaSenderService
{
    Task SendMessageAsync(string topic, object message);

    void Dispose();
}