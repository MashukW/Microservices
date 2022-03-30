using Mango.Services.PaymentAPI.Models.Messages;
using Mango.Services.PaymentAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.Message;
using Shared.Message.Options.RabbitMq;
using Shared.Message.Services.Interfaces;
using Shared.Options;
using System.Text.Json;

namespace Mango.Services.OrderAPI.MessageConsumers
{
    public class OrderPaymentRequestHandler : BackgroundService
    {
        private readonly MessageBusOptions _messageBusOptions;
        private readonly IPaymentService _paymentService;
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessagePublisher _messagePublisher;

        public OrderPaymentRequestHandler(
            IOptions<MessageBusOptions> messageBusOptions,
            IPaymentService paymentService,
            IMessageConsumer messageConsumer,
            IMessagePublisher messagePublisher)
        {
            _messageBusOptions = messageBusOptions.Value;
            _paymentService = paymentService;
            _messageConsumer = messageConsumer;
            _messagePublisher = messagePublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                await _messageConsumer.StopProcessing();
                stoppingToken.ThrowIfCancellationRequested();
            }

            // Azure
            // await _messageConsumer.StartProcessing(new AzureTopicConsumeMessageOptions
            // {
            //     ConnectionString = _messageBusOptions.ConnectionString,
            //     ConsumptionTopic = MessageConstants.Azure.Topics.OrderPaymentRequest,
            //     ConsumptionSubscription = MessageConstants.Azure.Subscriptions.OrderPaymentRequest,
            // }, HandleNewPaymentRequest);

            // RabbitMq
            await _messageConsumer.StartProcessing(new RabbitMqConsumeMessageOptions
            {
                HostName = _messageBusOptions.HostName,
                UserName = _messageBusOptions.UserName,
                Password = _messageBusOptions.Password,
                ConsumptionQueueName = MessageConstants.RabbitMq.Queues.OrderPaymentRequest
            }, HandleNewPaymentRequest);
        }


        public async Task HandleNewPaymentRequest(string jsonMessage)
        {
            try
            {
                var paymentRequestMessage = JsonSerializer.Deserialize<PaymentRequestMessage>(jsonMessage, JsonOptionsConfiguration.Options);

                if (paymentRequestMessage is not null)
                {
                    var isPaid = await _paymentService.HandlePayment();

                    var orderSuccessfullyPaidMessage = new OrderSuccessfullyPaidMessage
                    {
                        OrderId = paymentRequestMessage.OrderId,
                        Status = isPaid,
                        Email = "success@mymail.com"
                    };

                    // Azure
                    // await _messagePublisher.Publish(orderSuccessfullyPaidMessage, new AzurePublishOptions
                    // {
                    //     ConnectionString = _messageBusOptions.ConnectionString,
                    //     PublishTopicOrQueue = MessageConstants.Azure.Topics.OrderPaymentSuccess
                    // });

                    // RabbitMq
                    await _messagePublisher.Publish(orderSuccessfullyPaidMessage, new RabbitMqPublishOptions
                    {
                        HostName = _messageBusOptions.HostName,
                        UserName = _messageBusOptions.UserName,
                        Password = _messageBusOptions.Password,
                        ExchangeName = MessageConstants.RabbitMq.Exchanges.CheckoutFanout,
                        RoutingKey = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                // log exception
                Console.WriteLine(ex.Message);
            }
        }
    }
}