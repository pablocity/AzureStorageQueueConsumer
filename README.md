# Azure Storage Queue Consumer

Simple ASP.NET app with hosted service to listen for and consume Azure Storage Queue messages. It is using MediatR to provide clean way of resolving
and consuming different message types. Easily configurable with standard .NET configuration options. Inspired by Nick Chapsas's video about consuming messages from AWS SQS https://www.youtube.com/watch?v=wnqBmv1RJNE&t=734s 

Usage:

- Add proper settings for your queue (connection string, queue name, max message count and indicator whether messages are base64-encoded)
- Create your message object that will inherit from IMessage interface, it needs to have MessageType property (check out TestMessage class)
- Create handler class inheriting from IRequestHandler<YourMessageType> and implement Handle method to consume the message
- Optionally add logging and other stuff specific to your application