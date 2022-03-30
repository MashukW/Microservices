using RabbitMQ.Client;
using Shared.Message.Options.RabbitMq;

namespace Shared.Message.Services.RabbitMq
{
    public static class RabbitMqInitializer
    {
        public static void Initialization(RabbitMqMessageOptions options)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Direct ExchangeDeclare and QueueBind
            channel.ExchangeDeclare(exchange: MessageConstants.RabbitMq.Exchanges.CheckoutDirect, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

            var checkoutOrderQueue = channel.QueueDeclare(queue: MessageConstants.RabbitMq.Queues.CheckoutOrder, durable: true, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: checkoutOrderQueue, exchange: MessageConstants.RabbitMq.Exchanges.CheckoutDirect, routingKey: MessageConstants.RabbitMq.RoutingKeys.CheckoutOrder, arguments: null);

            var orderPaymentRequestQueue = channel.QueueDeclare(queue: MessageConstants.RabbitMq.Queues.OrderPaymentRequest, durable: true, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: orderPaymentRequestQueue, exchange: MessageConstants.RabbitMq.Exchanges.CheckoutDirect, routingKey: MessageConstants.RabbitMq.RoutingKeys.OrderPaymentRequest, arguments: null);


            // Fanout ExchangeDeclare and QueueBind
            channel.ExchangeDeclare(exchange: MessageConstants.RabbitMq.Exchanges.CheckoutFanout, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

            var orderPaymentSuccessUpdateStatusQueue = channel.QueueDeclare(queue: MessageConstants.RabbitMq.Queues.OrderPaymentSuccessUpdateStatus, durable: true, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: orderPaymentSuccessUpdateStatusQueue, exchange: MessageConstants.RabbitMq.Exchanges.CheckoutFanout, routingKey: "", arguments: null);

            var orderPaymentSuccessSendEmailQueue = channel.QueueDeclare(queue: MessageConstants.RabbitMq.Queues.OrderPaymentSuccessSendEmail, durable: true, exclusive: false, autoDelete: true, arguments: null);
            channel.QueueBind(queue: orderPaymentSuccessSendEmailQueue, exchange: MessageConstants.RabbitMq.Exchanges.CheckoutFanout, routingKey: "", arguments: null);
        }
    }
}

/*
    var directQueueName1 = channel.QueueDeclare(queue: "direct_queue_1", durable: false, exclusive: false, autoDelete: true, arguments: null);
    var directQueueName2 = channel.QueueDeclare(queue: "direct_queue_2", durable: false, exclusive: false, autoDelete: true, arguments: null);
    channel.QueueBind(queue: directQueueName1, exchange: "direct_exchange_sandbox", routingKey: "direct_queue_rk_1", arguments: null);
    channel.QueueBind(queue: directQueueName2, exchange: "direct_exchange_sandbox", routingKey: "direct_queue_rk_2", arguments: null);
    
    -> channel.BasicPublish(exchange: "direct_exchange_sandbox", routingKey: "direct_queue_rk_1", basicProperties: null, body: messageBody);
    
    
    // Fanout
    channel.ExchangeDeclare(exchange: "fanout_exchange_sandbox", type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);
    
    var fanoutQueueName1 = channel.QueueDeclare(queue: "fanout_queue_1", durable: false, exclusive: false, autoDelete: true, arguments: null);
    var fanoutQueueName2 = channel.QueueDeclare(queue: "fanout_queue_2", durable: false, exclusive: false, autoDelete: true, arguments: null);
    channel.QueueBind(queue: fanoutQueueName1, exchange: "fanout_exchange_sandbox", routingKey: "", arguments: null);
    channel.QueueBind(queue: fanoutQueueName2, exchange: "fanout_exchange_sandbox", routingKey: "", arguments: null);
    
    -> channel.BasicPublish(exchange: "fanout_exchange_sandbox", routingKey: "", basicProperties: null, body: messageBody);
    
    // Topic
    channel.ExchangeDeclare(exchange: "topic_exchange_sandbox", type: ExchangeType.Topic, durable: false, autoDelete: false, arguments: null);
    
    var topicQueueName1 = channel.QueueDeclare(queue: "topic_queue_1", durable: false, exclusive: false, autoDelete: true, arguments: null);
    var topicQueueName2 = channel.QueueDeclare(queue: "topic_queue_2", durable: false, exclusive: false, autoDelete: true, arguments: null);
    var topicQueueName3 = channel.QueueDeclare(queue: "topic_queue_3", durable: false, exclusive: false, autoDelete: true, arguments: null);
    var topicQueueName4 = channel.QueueDeclare(queue: "topic_queue_4", durable: false, exclusive: false, autoDelete: true, arguments: null);
    channel.QueueBind(queue: topicQueueName1, exchange: "topic_exchange_sandbox", routingKey: "routing.#", arguments: null);
    channel.QueueBind(queue: topicQueueName2, exchange: "topic_exchange_sandbox", routingKey: "routing.*", arguments: null);
    channel.QueueBind(queue: topicQueueName3, exchange: "topic_exchange_sandbox", routingKey: "*.key.*", arguments: null);
    channel.QueueBind(queue: topicQueueName4, exchange: "topic_exchange_sandbox", routingKey: "routing.key.test", arguments: null);
    
    -> channel.BasicPublish(exchange: "topic_exchange_sandbox", routingKey: "routing.key", basicProperties: null, body: messageBody);
*/
