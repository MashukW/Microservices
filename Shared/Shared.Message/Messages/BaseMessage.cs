namespace Shared.Message.Messages
{
    public class BaseMessage
    {
        public BaseMessage()
        {
            Id = string.Empty;
        }

        public string Id { get; set; }

        public DateTime Created { get; set; }
    }
}