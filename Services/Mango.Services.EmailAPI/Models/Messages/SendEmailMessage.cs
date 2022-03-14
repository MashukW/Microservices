using Shared.Message.Messages;

namespace Mango.Services.EmailAPI.Models.Messages
{
    public class SendEmailMessage : BaseMessage
    {
        public Guid OrderId { get; set; }

        public bool Status { get; set; }

        public string? Email { get; set; }
    }
}
