using Confluent.Kafka;
using RateLimiter.Reader.Models.Kafka;
using RateLimiter.Reader.Services.RateLimits;
using RateLimiter.Reader.Utils;

namespace RateLimiter.Reader.Services.Kafka;

public class KafkaConsumer : IHostedService
    {
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "events";
        private readonly string _groupId = "rate-limiter-consumer-group";
        private readonly IRateLimitChecker _rateLimitChecker;
        private IConsumer<Ignore, string>? _consumer;
        private Task? _consumingTask;
        private CancellationTokenSource? _cts;

        public KafkaConsumer(IRateLimitChecker rateLimitChecker)
        {
            _rateLimitChecker = rateLimitChecker;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(_topic);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _consumingTask = Task.Run(() => StartConsuming(_cts.Token), cancellationToken);

            Console.WriteLine("KafkaConsumerHostedService started.");
            return Task.CompletedTask;
        }

        private void StartConsuming(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = _consumer?.Consume(cancellationToken);
                    if (result is not null)
                    {
                        var kafkaEvent = JsonUtil.ParseKafkaEvent(result.Message.Value);
                        _rateLimitChecker.CheckRateLimitAsync(kafkaEvent);
                        Console.WriteLine($"Parsed: {kafkaEvent}");
                        
                        // Добавьте логику обработки сообщений здесь
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consuming cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while consuming messages: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("KafkaConsumerHostedService stopping...");
            _cts?.Cancel();

            _consumingTask ??= Task.CompletedTask;
            return _consumingTask.ContinueWith(_ =>
            {
                _consumer?.Close();
                Console.WriteLine("KafkaConsumerHostedService stopped.");
            }, cancellationToken);
        }
    }