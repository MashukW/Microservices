using Azure.Messaging.ServiceBus;

namespace Shared.Message.Services.Interfaces
{
    public interface IMessageConsumer
    {
        Task StartProcessing(string topic, string subscription, Func<ProcessMessageEventArgs, Task> processMessage, Func<ProcessErrorEventArgs, Task>? processError = null);

        Task StopProcessing();
    }
}
