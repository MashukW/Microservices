using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Message.Services.Interfaces;
using Shared.Options;

namespace Shared.Message.Services
{
    public class AzureMessageConsumer : IMessageConsumer
    {
        private readonly MessageBusOptions _messageBusOptions;
        private ServiceBusProcessor? _serviceBusProcessor;

        public AzureMessageConsumer(IOptions<MessageBusOptions> messageBusOptions)
        {
            _messageBusOptions = messageBusOptions.Value;
        }

        public async Task StartProcessing(string topic, string subscription, Func<ProcessMessageEventArgs, Task> processMessage, Func<ProcessErrorEventArgs, Task>? processError = null)
        {
            var client = new ServiceBusClient(_messageBusOptions.ConnectionString);
            _serviceBusProcessor = client.CreateProcessor(topic, subscription);

            _serviceBusProcessor.ProcessMessageAsync += processMessage;
            _serviceBusProcessor.ProcessErrorAsync += processError ?? ProcessError;

            await _serviceBusProcessor.StartProcessingAsync();
        }

        public async Task StopProcessing()
        {
            if (_serviceBusProcessor != null)
            {
                await _serviceBusProcessor.StopProcessingAsync();
                await _serviceBusProcessor.DisposeAsync();
            }
        }

        private Task ProcessError(ProcessErrorEventArgs messageArgs)
        {
            Console.WriteLine(messageArgs.Exception.ToString());
            return Task.CompletedTask;
        }

        public void StartProcessing(string topic, string subscription, EventHandler<BasicDeliverEventArgs> processMessage, EventHandler<ShutdownEventArgs>? processError = null)
        {
            throw new NotSupportedException();
        }
    }
}
