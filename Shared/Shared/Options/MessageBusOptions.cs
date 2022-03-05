namespace Shared.Options
{
    public class MessageBusOptions
    {
        public MessageBusOptions()
        {
            ConnectionString = string.Empty;
            Topic = string.Empty;
            Subscription = string.Empty;
        }

        public string ConnectionString { get; set; }

        public string Topic { get; set; }

        public string Subscription { get; set; }
    }
}
