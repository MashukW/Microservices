using Mango.Services.EmailAPI.Database;
using Mango.Services.EmailAPI.Database.Entities;
using Mango.Services.EmailAPI.Models.Messages;
using Mango.Services.EmailAPI.Services.Interfaces;
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
    public class SendEmailHandler : BackgroundService
    {
        private readonly MessageBusOptions _messageBusOptions;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        private readonly IMessageConsumer _messageConsumer;
        private readonly IEmailService _emailService;

        public SendEmailHandler(
            IOptions<MessageBusOptions> messageBusOptions,
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            IMessageConsumer messageConsumer,
            IEmailService emailService)
        {
            _messageBusOptions = messageBusOptions.Value;
            _dbContextOptions = dbContextOptions;
            _messageConsumer = messageConsumer;
            _emailService = emailService;
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
            //     ConsumptionTopic = MessageConstants.Azure.Topics.OrderPaymentSuccess,
            //     ConsumptionSubscription = MessageConstants.Azure.Subscriptions.OrderPaymentSuccessSendEmail,
            // }, HandleNewEmailRequest);

            // RabbitMq
            await _messageConsumer.StartProcessing(new RabbitMqConsumeMessageOptions
            {
                HostName = _messageBusOptions.HostName,
                UserName = _messageBusOptions.UserName,
                Password = _messageBusOptions.Password,
                ConsumptionQueueName = MessageConstants.RabbitMq.Queues.OrderPaymentSuccessSendEmail
            }, HandleNewEmailRequest);
        }


        public async Task HandleNewEmailRequest(string jsonMessage)
        {
            try
            {
                var sendEmailMessage = JsonSerializer.Deserialize<SendEmailMessage>(jsonMessage, JsonOptionsConfiguration.Options);

                if (sendEmailMessage is not null && sendEmailMessage.Email is not null && sendEmailMessage.Status is true)
                {
                    var isSent = await _emailService.Send(sendEmailMessage.Email);

                    var emailLog = new EmailLog
                    {
                        Email = sendEmailMessage.Email,
                        Log = $"Email message from order '{sendEmailMessage.OrderId}' was sent successfuly to {sendEmailMessage.Email}"
                    };

                    await using var database = new ApplicationDbContext(_dbContextOptions);
                    await database.Set<EmailLog>().AddAsync(emailLog);
                    await database.SaveChangesAsync();
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