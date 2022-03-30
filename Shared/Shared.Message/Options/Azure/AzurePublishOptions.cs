namespace Shared.Message.Options.Azure
{
    public class AzurePublishOptions : AzureMessageOptions
    {
        public string? PublishTopicOrQueue { get; set; }
    }
}
