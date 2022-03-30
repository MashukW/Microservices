using AutoMapper;
using Mango.Services.OrderAPI.Database;
using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.Models.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.Message;
using Shared.Message.Options.RabbitMq;
using Shared.Message.Services.Interfaces;
using Shared.Options;
using System.Text.Json;

namespace Mango.Services.OrderAPI.MessageConsumers
{
    public class CheckoutOrderHandler : BackgroundService
    {
        private readonly MessageBusOptions _messageBusOptions;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IMapper _mapper;

        public CheckoutOrderHandler(
            IOptions<MessageBusOptions> messageBusOptions,
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            IMessageConsumer messageConsumer,
            IMessagePublisher messagePublisher,
            IMapper mapper)
        {
            _messageBusOptions = messageBusOptions.Value;

            _dbContextOptions = dbContextOptions;

            _messageConsumer = messageConsumer;
            _messagePublisher = messagePublisher;

            _mapper = mapper;
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
            //     ConsumptionTopic = MessageConstants.Azure.Topics.CheckoutOrder,
            //     ConsumptionSubscription = MessageConstants.Azure.Subscriptions.CheckoutOrder,
            // }, HandleNewOrder);

            // RabbitMq
            await _messageConsumer.StartProcessing(new RabbitMqConsumeMessageOptions
            {
                HostName = _messageBusOptions.HostName,
                UserName = _messageBusOptions.UserName,
                Password = _messageBusOptions.Password,
                ConsumptionQueueName = MessageConstants.RabbitMq.Queues.CheckoutOrder
            }, HandleNewOrder);
        }

        public async Task HandleNewOrder(string jsonMessage)
        {
            try
            {
                var orderMessage = JsonSerializer.Deserialize<OrderMessage>(jsonMessage, JsonOptionsConfiguration.Options);

                var order = _mapper.Map<Order>(orderMessage);

                await using var database = new ApplicationDbContext(_dbContextOptions);
                await database.AddAsync(order);
                await database.SaveChangesAsync();

                var paymentRequestMessage = new PaymentRequestMessage
                {
                    OrderId = order.PublicId,
                    UserName = $"{order.FirstName} {order.LastName}",
                    CardNumber = order.CardNumber,
                    Cvv = order.Cvv,
                    ExpityMonthYear = order.ExpityMonthYear,
                    OrderTotalCost = order.TotalCost,
                    Created = DateTime.UtcNow,
                    Email = order.Email,
                };

                // Azure
                // await _messagePublisher.Publish(paymentRequestMessage, new AzurePublishOptions
                // {
                //     ConnectionString = _messageBusOptions.ConnectionString,
                //     PublishTopicOrQueue = MessageConstants.Azure.Topics.OrderPaymentRequest
                // });

                // RabbitMq
                await _messagePublisher.Publish(paymentRequestMessage, new RabbitMqPublishOptions
                {
                    HostName = _messageBusOptions.HostName,
                    UserName = _messageBusOptions.UserName,
                    Password = _messageBusOptions.Password,
                    ExchangeName = MessageConstants.RabbitMq.Exchanges.CheckoutDirect,
                    RoutingKey = MessageConstants.RabbitMq.RoutingKeys.OrderPaymentRequest
                });
            }
            catch (Exception ex)
            {
                // log exception
                Console.WriteLine(ex.Message);
            }
        }
    }
}