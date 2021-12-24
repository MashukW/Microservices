namespace Shared.Options
{
    public class MessageBusOptions
    {
        public MessageBusOptions()
        {
            ConnectionString = string.Empty;
        }

        public string ConnectionString { get; set; }
    }
}
