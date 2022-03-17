using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Message.Services.Interfaces;
using Shared.Options;

namespace Shared.Message.Services.RabbitMq
{
    public class RabbitMqMessageConsumer : IMessageConsumer
    {
        private readonly MessageBusOptions _messageBusOptions;
        private IConnection? _connection;

        public RabbitMqMessageConsumer(IOptions<MessageBusOptions> messageBusOptions)
        {
            _messageBusOptions = messageBusOptions.Value;
        }

        public Task StartProcessing(string topic, string subscription, Func<ProcessMessageEventArgs, Task> processMessage, Func<ProcessErrorEventArgs, Task>? processError = null)
        {
            throw new NotImplementedException();
        }

        public Task StopProcessing()
        {
            throw new NotImplementedException();
        }
    }
}
