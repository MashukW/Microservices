using Shared.Message.Messages;

namespace Mango.Services.PaymentAPI.Models.Messages
{
    public class UpdatePaymentStatusMessage : BaseMessage
    {
        public Guid OrderId { get; set; }

        public bool Status { get; set; }
    }
}
