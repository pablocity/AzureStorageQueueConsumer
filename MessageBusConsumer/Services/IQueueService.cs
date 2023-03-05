using Azure.Storage.Queues.Models;

namespace MessageBusConsumer;

public interface IQueueService
{
    Task<IEnumerable<QueueMessage>> GetMessagesAsync(int? maxMessageCount = null, CancellationToken cancellationToken = default);
    Task<bool> ConsumeMessageAsync(QueueMessage message);
}