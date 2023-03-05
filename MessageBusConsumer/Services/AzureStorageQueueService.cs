using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace MessageBusConsumer;

public class AzureStorageQueueService : IQueueService
{
    private readonly QueueClient _queueClient;
    private readonly AzureStorageQueueOptions _queueOptions;
    public AzureStorageQueueService(IOptions<AzureStorageQueueOptions> options)
    {
        _queueOptions = options.Value;
        _queueClient = new QueueClient(_queueOptions.ConnectionString, _queueOptions.QueueName);
    }
    public async Task<bool> ConsumeMessageAsync(QueueMessage message)
    {
        var response = await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        //Log response if error
        return !response.IsError;
    }

    public async Task<IEnumerable<QueueMessage>> GetMessagesAsync(int? maxMessageCount = null, CancellationToken cancellationToken = default)
    {
        var messages = await _queueClient.ReceiveMessagesAsync(maxMessages: maxMessageCount, cancellationToken: cancellationToken);
        if (messages is null || messages?.Value?.Length <= 0)
            return Enumerable.Empty<QueueMessage>();

        return messages.Value;
    }
}
