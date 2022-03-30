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
        }

        // RabbitMq
        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        // Azure
        public string ConnectionString { get; set; }
    }
}
