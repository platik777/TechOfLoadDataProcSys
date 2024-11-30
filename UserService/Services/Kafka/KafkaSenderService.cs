using System.Text.Json;
using Confluent.Kafka;

namespace UserService.Services.Kafka;

public class KafkaSenderService : IKafkaSenderService
{
    private readonly IProducer<Null, string> _producer;

    public KafkaSenderService(string bootstrapServers)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public async Task SendMessageAsync(string topic, object message)
    {
        var serializedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var kafkaMessage = new Message<Null, string> { Value = serializedMessage };

        await _producer.ProduceAsync(topic, kafkaMessage);
        Console.WriteLine($"Sent to topic '{topic}': {serializedMessage}");
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}