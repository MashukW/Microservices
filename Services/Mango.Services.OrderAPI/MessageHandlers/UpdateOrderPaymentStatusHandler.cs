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
    public static class UpdateOrderPaymentStatusHandler
    {
        private static IMessageConsumer? _messageConsumer;
        private static IOptions<MessageBusOptions>? _messageBusOptions;
        private static DbContextOptions<ApplicationDbContext>? _dbContextOptions;

        public static IApplicationBuilder UseOrderPaymentSuccessUpdateStatusHandler(this IApplicationBuilder builder)
        {
            _dbContextOptions = builder.ApplicationServices.GetService<DbContextOptions<ApplicationDbContext>>();

            using (var scope = builder.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider is null)
                    throw new Exception();

                _messageConsumer = scope.ServiceProvider.GetService<IMessageConsumer>();
                _messageBusOptions = scope.ServiceProvider.GetService<IOptions<MessageBusOptions>>();

                var hostApplicationLife = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                hostApplicationLife?.ApplicationStarted.Register(async () => await OnStartConsuming());
                hostApplicationLife?.ApplicationStarted.Register(async () => await OnStopConsuming());
            }

            return builder;
        }

        private static async Task OnStartConsuming()
        {
            if (_messageConsumer is null)
                throw new Exception();

            if (_messageBusOptions is null)
                throw new Exception();

            if (_dbContextOptions is null)
                throw new Exception();

            // Azure
            // await _messageConsumer.StartProcessing(new AzureTopicConsumeMessageOptions
            // {
            //     ConnectionString = _messageBusOptions.Value.ConnectionString,
            //     ConsumptionTopic = MessageConstants.Azure.Topics.OrderPaymentSuccess,
            //     ConsumptionSubscription = MessageConstants.Azure.Subscriptions.OrderPaymentSuccessUpdateStatus,
            // }, UpdateOrderPaymentStatus);

            // RabbitMq
            await _messageConsumer.StartProcessing(new RabbitMqConsumeMessageOptions
            {
                HostName = _messageBusOptions.Value.HostName,
                UserName = _messageBusOptions.Value.UserName,
                Password = _messageBusOptions.Value.Password,
                ConsumptionQueueName = MessageConstants.RabbitMq.Queues.OrderPaymentSuccessUpdateStatus
            }, UpdateOrderPaymentStatus);
        }

        private static async Task OnStopConsuming()
        {
            if (_messageConsumer is null)
                throw new Exception();

            await _messageConsumer.StopProcessing();
        }

        public static async Task UpdateOrderPaymentStatus(string jsonMessage)
        {
            if (_dbContextOptions is null)
                throw new Exception();

            try
            {
                var updatePaymentStatusMessage = JsonSerializer.Deserialize<UpdatePaymentStatusMessage>(jsonMessage, JsonOptionsConfiguration.Options);

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
