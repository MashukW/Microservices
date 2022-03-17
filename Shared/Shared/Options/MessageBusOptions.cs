namespace Shared.Options
{
    public class MessageBusOptions
    {
        public MessageBusOptions()
        {
            HostName = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            ConnectionString = string.Empty;
            Topic = string.Empty;
            Subscription = string.Empty;
        }

        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConnectionString { get; set; }

        public string Topic { get; set; }

        public string Subscription { get; set; }
    }
}
