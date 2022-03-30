using Azure.Messaging.ServiceBus;
using Shared.Configurations;
using Shared.Message.Messages;
using Shared.Message.Options;
using Shared.Message.Options.Azure;
using Shared.Message.Services.Interfaces;
using System.Text.Json;

namespace Shared.Message.Services.Azure
{
    public class AzureMessagePublisher : IMessagePublisher
    {
        public async Task Publish<T>(T message, IMessageOptions options) where T : BaseMessage
        {
            if (options is AzurePublishOptions azurePublishOptions)
            {
                var messageJson = JsonSerializer.Serialize(message, JsonOptionsConfiguration.Options);

                await using var client = new ServiceBusClient(azurePublishOptions.ConnectionString);
                var sender = client.CreateSender(azurePublishOptions.PublishTopicOrQueue);

                var messagesToSend = new ServiceBusMessage(messageJson)
                {
                    CorrelationId = Guid.NewGuid().ToString("D")
                };

                await sender.SendMessageAsync(messagesToSend);
                await sender.DisposeAsync();
            }
            else
            {
                throw new NotSupportedException($"Type '{options.GetType().FullName}' does not support.");
            }
        }
    }
}
