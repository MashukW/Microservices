using RabbitMQ.Client;
using Shared.Configurations;
using Shared.Message.Messages;
using Shared.Message.Options;
using Shared.Message.Options.RabbitMq;
using Shared.Message.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Shared.Message.Services.RabbitMq
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        public Task Publish<T>(T message, IMessageOptions options) where T : BaseMessage
        {
            if (options is RabbitMqPublishOptions rabbitMqPublishOptions)
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitMqPublishOptions.HostName,
                    UserName = rabbitMqPublishOptions.UserName,
                    Password = rabbitMqPublishOptions.Password
                };

                var messageJson = JsonSerializer.Serialize(message, JsonOptionsConfiguration.Options);
                var messageBody = Encoding.UTF8.GetBytes(messageJson);

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.BasicPublish(exchange: rabbitMqPublishOptions.ExchangeName, routingKey: rabbitMqPublishOptions.RoutingKey, basicProperties: null, body: messageBody);

                // // Topic -> exchange and routing key (with pattern '*' -> one '#' -> zero or more words )
                // channel.BasicPublish(exchange: "topic_exchange_sandbox", routingKey: "routing.key", basicProperties: null, body: messageBody);
                // 
                // // Fanout -> exchange
                // channel.BasicPublish(exchange: "fanout_exchange_sandbox", routingKey: "", basicProperties: null, body: messageBody);
                // 
                // // Direct -> exchange and routingKey (without pattern)
                // channel.BasicPublish(exchange: "direct_exchange_sandbox", routingKey: "direct_queue_rk_1", basicProperties: null, body: messageBody);
            }
            else
            {
                throw new NotSupportedException($"Type '{options.GetType().FullName}' does not support.");
            }

            return Task.CompletedTask;
        }
    }
}
