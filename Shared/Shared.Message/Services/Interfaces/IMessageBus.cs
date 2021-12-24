using Shared.Message.Messages;

namespace Shared.Message.Services.Interfaces
{
    public interface IMessageBus
    {
        Task Publish<T>(T message, string topicName) where T : BaseMessage;
    }
}
