using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Database;
using Mango.Services.EmailAPI.Database.Entities;
using Mango.Services.EmailAPI.Models.Messages;
using Mango.Services.EmailAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Configurations;
using Shared.Message.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mango.Services.PaymentAPI.Services
{
    public class EmailMessageConsumer : IEmailMessageConsumer
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailService _emailService;

        public EmailMessageConsumer(
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            IMessageConsumer messageConsumer,
            IMessagePublisher messagePublisher,
            IEmailService emailService)
        {
            _dbContextOptions = dbContextOptions;

            _messageConsumer = messageConsumer;
            _messagePublisher = messagePublisher;

            _emailService = emailService;
        }

        public async Task Start()
        {
            await _messageConsumer.StartProcessing("update-order-payment-status", "send-email-subscription", ProcessNewEmailRequest);
        }

        public async Task Stop()
        {
            await _messageConsumer.StopProcessing();
        }

        public async Task ProcessNewEmailRequest(ProcessMessageEventArgs messageArgs)
        {
            try
            {
                var message = messageArgs.Message;
                var messageBody = Encoding.UTF8.GetString(message.Body);

                var sendEmailMessage = JsonSerializer.Deserialize<SendEmailMessage>(messageBody, JsonOptionsConfiguration.Options);

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

                    await messageArgs.CompleteMessageAsync(messageArgs.Message);
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
