namespace Shared.Message.Options.RabbitMq
{
    public class RabbitMqConsumeMessageOptions : RabbitMqMessageOptions
    {
        public string? ConsumptionQueueName { get; set; }
    }
}
