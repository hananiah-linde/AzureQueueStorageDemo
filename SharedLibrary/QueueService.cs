using System.Text.Json;
using Azure.Storage.Queues;

namespace SharedLibrary;

public interface IQueueService
{
    Task SendMessageAsync<T>(string queueName, T message);
}

public class QueueService : IQueueService
{
    private readonly QueueServiceClient _queueServiceClient;

    public QueueService(QueueServiceClient queueServiceClient)
    {
        _queueServiceClient = queueServiceClient;
    }

    public async Task SendMessageAsync<T>(string queueName, T message)
    {
        var queueClient = _queueServiceClient.GetQueueClient(queueName);
        await queueClient.CreateIfNotExistsAsync(); //for demo

        var json = JsonSerializer.Serialize(message);
        await queueClient.SendMessageAsync(json);
    }
}