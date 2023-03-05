using MediatR;
using System.Diagnostics;

namespace MessageBusConsumer.MessageHandlers.TestMessage;

public class TestMessageHandler : IRequestHandler<TestMessage>
{
    public Task Handle(TestMessage request, CancellationToken cancellationToken)
    {
        Debug.WriteLine("In test message handler");
        Debug.WriteLine(request);

        return Task.CompletedTask;
    }
}
