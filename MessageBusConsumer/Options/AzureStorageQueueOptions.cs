namespace MessageBusConsumer
{
    public class AzureStorageQueueOptions
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public int MaxMessageCount { get; set; } = 10;
        public bool IsMessageEncoded { get; set; }
    }
}
