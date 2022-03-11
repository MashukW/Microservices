using Shared.Message.Messages;

namespace Mango.Services.OrderAPI.Models.Messages
{
    public class UpdatePaymentStatusMessage : BaseMessage
    {
        public Guid OrderId { get; set; }

        public bool Status { get; set; }
    }
}
