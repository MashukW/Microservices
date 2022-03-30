namespace Shared.Message.Options.Azure
{
    public class AzureQueueConsumeMessageOptions : AzureMessageOptions
    {
        public string? ConsumptionQueue { get; set; }
    }

    public class AzureTopicConsumeMessageOptions : AzureMessageOptions
    {
        public string? ConsumptionTopic { get; set; }

        public string? ConsumptionSubscription { get; set; }
    }
}
