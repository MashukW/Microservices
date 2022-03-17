using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Configurations;
using Shared.Message.Messages;
using Shared.Message.Services.Interfaces;
using Shared.Options;
using System.Text;
using System.Text.Json;

namespace Shared.Message.Services.RabbitMq
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly MessageBusOptions _messageBusOptions;
        private IConnection? _connection;

        public RabbitMqMessagePublisher(IOptions<MessageBusOptions> messageBusOptions)
        {
            _messageBusOptions = messageBusOptions.Value;
        }

        public Task Publish<T>(T message, string topicName) where T : BaseMessage
        {
            var factory = new ConnectionFactory
            {
                HostName = _messageBusOptions.HostName,
                UserName = _messageBusOptions.UserName,
                Password = _messageBusOptions.Password
            };

            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: topicName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonSerializer.Serialize(message, JsonOptionsConfiguration.Options);
            var messageBody = Encoding.UTF8.GetBytes(messageJson);

            channel.BasicPublish(exchange: "", routingKey: topicName, basicProperties: null, body: messageBody);

            throw new NotImplementedException();
        }
    }
}
