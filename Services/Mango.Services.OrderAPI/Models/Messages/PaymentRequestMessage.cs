using Shared.Message.Messages;

namespace Mango.Services.OrderAPI.Models.Messages
{
    public class PaymentRequestMessage : BaseMessage
    {
        public PaymentRequestMessage()
        {
            UserName = string.Empty;
            CardNumber = string.Empty;
            Cvv = string.Empty;
            ExpityMonthYear = string.Empty;
        }

        public Guid OrderId { get; set; }

        public string UserName { get; set; }

        public string CardNumber { get; set; }

        public string Cvv { get; set; }

        public string ExpityMonthYear { get; set; }

        public double OrderTotalCost { get; set; }
    }
}
