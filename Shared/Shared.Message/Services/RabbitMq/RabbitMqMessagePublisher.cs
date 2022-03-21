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

        public async Task Publish<T>(T message, string topicName) where T : BaseMessage
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: topicName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var messageJson = JsonSerializer.Serialize(message, JsonOptionsConfiguration.Options);
                var messageBody = Encoding.UTF8.GetBytes(messageJson);

                await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: topicName, basicProperties: null, body: messageBody));
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _messageBusOptions.HostName,
                    UserName = _messageBusOptions.UserName,
                    Password = _messageBusOptions.Password
                };

                _connection = factory.CreateConnection();

            }
            catch (Exception ex)
            {
                // log exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();
            return _connection != null;
        }
    }
}
