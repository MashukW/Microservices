using Shared.Message.Messages;

namespace Mango.Services.PaymentAPI.Models.Messages
{
    public class OrderSuccessfullyPaidMessage : BaseMessage
    {
        public Guid OrderId { get; set; }

        public bool Status { get; set; }

        public string? Email { get; set; }
    }
}
