using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Message.Options;
using Shared.Message.Options.RabbitMq;
using Shared.Message.Services.Interfaces;
using System.Text;

namespace Shared.Message.Services.RabbitMq
{
    public class RabbitMqMessageConsumer : IMessageConsumer
    {
        private IConnection? _connection;
        private IModel? _channel;

        public Task StartProcessing(IMessageOptions options, Func<string, Task> handleMessage, Func<string, Task>? handleError = null)
        {
            if (options is RabbitMqConsumeMessageOptions rabbitMqConsumeMessageOptions)
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = rabbitMqConsumeMessageOptions.HostName,
                    UserName = rabbitMqConsumeMessageOptions.UserName,
                    Password = rabbitMqConsumeMessageOptions.Password,
                };

                _connection = connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();
                // _channel.QueueDeclare(queue: rabbitMqConsumeMessageOptions.ConsumptionQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (sender, eventArgs) =>
                {
                    var message = eventArgs.Body;
                    var messageBody = Encoding.UTF8.GetString(message.ToArray());

                    await handleMessage(messageBody);

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                };
                consumer.Shutdown += async (sender, eventArgs) =>
                {
                    var errorMessage = eventArgs.Cause?.ToString();

                    if (handleError is null)
                        Console.WriteLine(errorMessage);
                    else
                        await handleError(errorMessage ?? "Identified error");
                };

                _channel.BasicConsume(queue: rabbitMqConsumeMessageOptions.ConsumptionQueueName, autoAck: false, consumer);
            }
            else
            {
                throw new NotSupportedException($"Type '{options.GetType().FullName}' does not support. Method support 'queue' and 'topic' only.");
            }

            return Task.CompletedTask;
        }

        public async Task StopProcessing()
        {
            if (_connection != null)
            {
                _connection.Abort();
            }

            await Task.CompletedTask;
        }
    }
}
