using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.Message.Messages;
using Shared.Message.Services.Interfaces;
using Shared.Options;
using System.Text.Json;

namespace Shared.Message.Services
{
    public class AzureMessagePublisher : IMessagePublisher
    {
        private readonly MessageBusOptions _messageBusOptions;

        public AzureMessagePublisher(IOptions<MessageBusOptions> messageBusOptions)
        {
            _messageBusOptions = messageBusOptions.Value;
        }

        public async Task Publish<T>(T message, string topicName) where T : BaseMessage
        {
            var messageJson = JsonSerializer.Serialize(message, JsonOptionsConfiguration.Options);

            await using var client = new ServiceBusClient(_messageBusOptions.ConnectionString);
            var sender = client.CreateSender(topicName);

            var messagesToSend = new ServiceBusMessage(messageJson)
            {
                CorrelationId = Guid.NewGuid().ToString("D")
            };

            await sender.SendMessageAsync(messagesToSend);
            await sender.DisposeAsync();
        }
    }
}
