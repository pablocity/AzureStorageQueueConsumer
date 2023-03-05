using MediatR;

namespace MessageBusConsumer;

public interface IMessage : IRequest
{
    public string MessageType { get; init; }
}