namespace Shared.Message.Options.RabbitMq
{
    public class RabbitMqPublishOptions : RabbitMqMessageOptions
    {
        public string? ExchangeName { get; set; }

        public string? RoutingKey { get; set; }
    }
}
