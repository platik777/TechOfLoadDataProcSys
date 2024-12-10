using System.Collections.Concurrent;
using System.Text.Json;
using Confluent.Kafka;

namespace UserService.Services.Kafka;

public class KafkaHostedService : IHostedService, IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly ConcurrentDictionary<string, TaskData> _tasks = new();
    private readonly CancellationTokenSource _cts = new();

    public KafkaHostedService(string bootstrapServers)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        return Task.WhenAll(_tasks.Values.Select(taskData => taskData.Task));
    }

    public void AddOrUpdateTask(string key, int rpm, object message)
    {
        if (_tasks.TryRemove(key, out var existingTask))
        {
            existingTask.CancellationTokenSource.Cancel();
        }

        var newCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);

        var task = Task.Run(async () =>
        {
            while (!newCts.Token.IsCancellationRequested)
            {
                try
                {
                    var serializedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    await _producer.ProduceAsync("events", new Message<Null, string> { Value = serializedMessage }, newCts.Token);

                    await Task.Delay(60000 / rpm, newCts.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to Kafka: {ex.Message}");
                }
            }
        }, newCts.Token);

        _tasks[key] = new TaskData
        {
            Task = task,
            CancellationTokenSource = newCts
        };
    }

    public void RemoveTask(string key)
    {
        if (_tasks.TryRemove(key, out var taskData))
        {
            taskData.CancellationTokenSource.Cancel();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _producer.Dispose();
        foreach (var taskData in _tasks.Values)
        {
            taskData.CancellationTokenSource.Dispose();
        }
    }

    private class TaskData
    {
        public Task Task { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
