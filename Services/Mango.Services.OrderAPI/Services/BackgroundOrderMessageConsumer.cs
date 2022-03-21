using AutoMapper;
using Mango.Services.OrderAPI.Database;
using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.Models.Messages;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using Shared.Configurations;
using Shared.Message.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mango.Services.OrderAPI.Services
{
    public class BackgroundOrderMessageConsumer : BackgroundService
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessagePublisher _messagePublisher;

        private readonly IMapper _mapper;

        public BackgroundOrderMessageConsumer(
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _messageConsumer.StopProcessing();
                stoppingToken.ThrowIfCancellationRequested();
            }

            _messageConsumer.StartProcessing("checkout", "", ProcessNewOrder);

            return Task.CompletedTask;
        }

        private async void ProcessNewOrder(object? sender, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var messageBody = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

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
                    Created = DateTime.UtcNow,
                    Email = order.Email,
                };

                await _messagePublisher.Publish(paymentRequestMessage, "order-payment-request");
            }
            catch (Exception ex)
            {
                // log exception
                Console.WriteLine(ex.Message);
            }
        }
    }
}
