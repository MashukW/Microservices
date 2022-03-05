using Shared.Message.Messages;

namespace Shared.Message.Services.Interfaces
{
    public interface IMessagePublisher
    {
        Task Publish<T>(T message, string topicName) where T : BaseMessage;
    }
}
