using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Message.Services.Interfaces;
using Shared.Options;

namespace Shared.Message.Services.RabbitMq
{
    public class RabbitMqMessageConsumer : IMessageConsumer
    {
        private readonly MessageBusOptions _messageBusOptions;
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqMessageConsumer(IOptions<MessageBusOptions> messageBusOptions)
        {
            _messageBusOptions = messageBusOptions.Value;
        }

        public void StartProcessing(string topic, string subscription, EventHandler<BasicDeliverEventArgs> processMessage, EventHandler<ShutdownEventArgs>? processError = null)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _messageBusOptions.HostName,
                UserName = _messageBusOptions.UserName,
                Password = _messageBusOptions.Password,
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: topic, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                processMessage(sender, eventArgs);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            consumer.Shutdown += processError ?? ProcessError;

            _channel.BasicConsume(topic, autoAck: false, consumer);
        }

        public async Task StopProcessing()
        {
            if (_connection != null)
            {
                _connection.Abort();
            }

            await Task.CompletedTask;
        }

        private void ProcessError(object? sender, ShutdownEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Cause);
        }

        public Task StartProcessing(string topic, string subscription, Func<ProcessMessageEventArgs, Task> processMessage, Func<ProcessErrorEventArgs, Task>? processError = null)
        {
            throw new NotSupportedException();
        }
    }
}
