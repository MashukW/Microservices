using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Services.Interfaces;
using Mango.Services.PaymentAPI.Models.Messages;
using Mango.Services.PaymentAPI.Services.Interfaces;
using Shared.Configurations;
using Shared.Message.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mango.Services.PaymentAPI.Services
{
    public class PaymentMessageConsumer : IPaymentMessageConsumer
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IPaymentService _paymentService;

        public PaymentMessageConsumer(
            IMessageConsumer messageConsumer,
            IMessagePublisher messagePublisher,
            IPaymentService paymentService)
        {
            _messageConsumer = messageConsumer;
            _messagePublisher = messagePublisher;
            _paymentService = paymentService;
        }

        public async Task Start()
        {
            await _messageConsumer.StartProcessing("order-payment-request", "order-payment-request-subscription", ProcessNewPaymentRequest);
        }

        public async Task Stop()
        {
            await _messageConsumer.StopProcessing();
        }

        public async Task ProcessNewPaymentRequest(ProcessMessageEventArgs messageArgs)
        {
            try
            {
                var message = messageArgs.Message;
                var messageBody = Encoding.UTF8.GetString(message.Body);

                var paymentRequestMessage = JsonSerializer.Deserialize<PaymentRequestMessage>(messageBody, JsonOptionsConfiguration.Options);

                if (paymentRequestMessage is not null)
                {
                    var isPaid = await _paymentService.HandlePayment();

                    var updatePaymentStatusMessage = new UpdatePaymentStatusMessage
                    {
                        OrderId = paymentRequestMessage.OrderId,
                        Status = isPaid
                    };

                    await _messagePublisher.Publish(updatePaymentStatusMessage, "update-order-payment-status");
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
