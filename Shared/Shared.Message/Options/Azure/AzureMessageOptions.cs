namespace Shared.Message.Options.Azure
{
    public abstract class AzureMessageOptions : IMessageOptions
    {
        public string? ConnectionString { get; set; }
    }
}
