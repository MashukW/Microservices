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
    public class UpdatePaymentStatusMessageConsumer : IUpdatePaymentStatusMessageConsumer
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IMessageConsumer _messageConsumer;

        public UpdatePaymentStatusMessageConsumer(
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            IMessageConsumer messageConsumer)
        {
            _dbContextOptions = dbContextOptions;
            _messageConsumer = messageConsumer;
        }

        public async Task Start()
        {
            await _messageConsumer.StartProcessing("update-order-payment-status", "update-order-payment-status-subscription", ProcessUpdatePaymentStatus);
        }

        public async Task Stop()
        {
            await _messageConsumer.StopProcessing();
        }

        public async Task ProcessUpdatePaymentStatus(ProcessMessageEventArgs messageArgs)
        {
            try
            {
                var message = messageArgs.Message;
                var messageBody = Encoding.UTF8.GetString(message.Body);

                var updatePaymentStatusMessage = JsonSerializer.Deserialize<UpdatePaymentStatusMessage>(messageBody, JsonOptionsConfiguration.Options);

                if (updatePaymentStatusMessage is not null)
                {
                    await using var database = new ApplicationDbContext(_dbContextOptions);

                    var order = await database.Set<Order>().FirstOrDefaultAsync(x => x.PublicId == updatePaymentStatusMessage.OrderId);

                    if (order is not null)
                    {
                        order.PaymentStatus = updatePaymentStatusMessage.Status;

                        database.Update(order);
                        await database.SaveChangesAsync();
                    }
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
