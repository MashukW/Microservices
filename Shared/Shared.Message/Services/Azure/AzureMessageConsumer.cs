using Azure.Messaging.ServiceBus;
using Shared.Message.Options;
using Shared.Message.Options.Azure;
using Shared.Message.Services.Interfaces;
using System.Text;

namespace Shared.Message.Services.Azure
{
    public class AzureMessageConsumer : IMessageConsumer
    {
        private ServiceBusProcessor? _serviceBusProcessor;

        public async Task StartProcessing(IMessageOptions options, Func<string, Task> handleMessage, Func<string, Task>? handleError = null)
        {
            if (options is AzureTopicConsumeMessageOptions azureTopicConsumeOptions)
            {
                var client = new ServiceBusClient(azureTopicConsumeOptions.ConnectionString);
                _serviceBusProcessor = client.CreateProcessor(azureTopicConsumeOptions.ConsumptionTopic, azureTopicConsumeOptions.ConsumptionSubscription);
            }
            else if (options is AzureQueueConsumeMessageOptions azureQueueConsumeOptions)
            {
                var client = new ServiceBusClient(azureQueueConsumeOptions.ConnectionString);
                _serviceBusProcessor = client.CreateProcessor(azureQueueConsumeOptions.ConsumptionQueue);
            }
            else
            {
                throw new NotSupportedException($"Type '{options.GetType().FullName}' does not support. Method support 'queue' and 'topic' only.");
            }

            _serviceBusProcessor.ProcessMessageAsync += async (args) =>
            {
                var message = args.Message;
                var messageBody = Encoding.UTF8.GetString(message.Body);

                await handleMessage(messageBody);

                await args.CompleteMessageAsync(args.Message);
            };

            _serviceBusProcessor.ProcessErrorAsync += async (args) =>
            {
                var errorMessage = args.Exception.ToString();

                if (handleError is null)
                    Console.WriteLine(errorMessage);
                else
                    await handleError(errorMessage);
            };

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
    }
}
