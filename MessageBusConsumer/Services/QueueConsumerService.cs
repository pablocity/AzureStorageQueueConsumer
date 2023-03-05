using Azure.Core;
using MediatR;
using MessageBusConsumer.MessageHandlers.TestMessage;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace MessageBusConsumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IQueueService _queueService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AzureStorageQueueOptions _queueOptions;
    private readonly Dictionary<string, Type> _messageTypes;
    public QueueConsumerService(
        IQueueService queueService,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AzureStorageQueueOptions> queueOptions)
    {
        _queueService = queueService;
        _serviceScopeFactory = serviceScopeFactory;
        _queueOptions = queueOptions.Value;
        _messageTypes = typeof(Program).Assembly.DefinedTypes
            .Where(x =>
                typeof(IMessage).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .ToDictionary(type => type.Name, type => type.AsType());
    }
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            var messages = await _queueService.GetMessagesAsync(_queueOptions.MaxMessageCount, stoppingToken);

            foreach (var message in messages)
            {
                var decodedMessage = MessageDecoder.GetDecodedMessage(message, _queueOptions.IsMessageEncoded);

                var messageType = _messageTypes.GetValueOrDefault(decodedMessage.MessageType ?? String.Empty);

                if (messageType is null)
                {
                    // Log message type not found, continue not to consume message
                    continue;
                }

                var request = (IMessage)JsonSerializer.Deserialize(decodedMessage.JsonBody, messageType)!;

                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetService<IMediator>();
                        await mediator.Send(request, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    //TODO Log when exception while handling message occurs, continue not to consume message
                    continue;
                }

                await _queueService.ConsumeMessageAsync(message);
            }
        }
    }
}
