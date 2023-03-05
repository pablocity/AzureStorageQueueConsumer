using Azure.Storage.Queues.Models;
using System.Text;
using System.Text.Json;

namespace MessageBusConsumer
{
    public static class MessageDecoder
    {
        public static (string MessageType, string JsonBody) GetDecodedMessage(QueueMessage message, bool isMessageEncoded = true)
        {
            var jsonBody = isMessageEncoded ? Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText)) : message.MessageText;
            var messageObj = JsonSerializer.Deserialize<Message>(jsonBody);

            if (messageObj is null)
                return (String.Empty, String.Empty);

            return (messageObj.MessageType, jsonBody);
        }
    }
}
