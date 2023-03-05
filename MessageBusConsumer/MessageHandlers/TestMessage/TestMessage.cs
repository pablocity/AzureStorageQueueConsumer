using MediatR;

namespace MessageBusConsumer.MessageHandlers.TestMessage;

public class TestMessage : IMessage
{
    public string MessageId { get; init; }
    public string MessageType { get; init; }
    public string MessageName { get; init; }
    public int RandomInt { get; init; }
}
