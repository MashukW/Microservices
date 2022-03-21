using Azure.Messaging.ServiceBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shared.Message.Services.Interfaces
{
    public interface IMessageConsumer
    {
        Task StartProcessing(string topic, string subscription, Func<ProcessMessageEventArgs, Task> processMessage, Func<ProcessErrorEventArgs, Task>? processError = null);

        void StartProcessing(string topic, string subscription, EventHandler<BasicDeliverEventArgs> processMessage, EventHandler<ShutdownEventArgs>? processError = null);

        Task StopProcessing();
    }
}
