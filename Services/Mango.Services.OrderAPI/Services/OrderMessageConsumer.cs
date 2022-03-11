using AutoMapper;
using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Database;
using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.Models.Messages;
using Mango.Services.OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Configurations;
using Shared.Message.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mango.Services.OrderAPI.Services
{
    public class OrderMessageConsumer : IOrderMessageConsumer
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IMapper _mapper;

        public OrderMessageConsumer(
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            IMessageConsumer messageConsumer,
            IMessagePublisher messagePublisher,
            IMapper mapper)
        {
            _dbContextOptions = dbContextOptions;

            _messageConsumer = messageConsumer;
            _messagePublisher = messagePublisher;

            _mapper = mapper;
        }

        public async Task Start()
        {
            await _messageConsumer.StartProcessing("checkout", "checkout-order-subscription", ProcessNewOrder);
        }

        public async Task Stop()
        {
            await _messageConsumer.StopProcessing();
        }

        public async Task ProcessNewOrder(ProcessMessageEventArgs messageArgs)
        {
            try
            {
                var message = messageArgs.Message;
                var messageBody = Encoding.UTF8.GetString(message.Body);

                var orderMessage = JsonSerializer.Deserialize<OrderMessage>(messageBody, JsonOptionsConfiguration.Options);

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
                    Created = DateTime.UtcNow
                };

                await _messagePublisher.Publish(paymentRequestMessage, "order-payment-request");
                await messageArgs.CompleteMessageAsync(messageArgs.Message);
            }
            catch (Exception ex)
            {
                // log exception
                Console.WriteLine(ex.Message);
            }
        }
    }
}
