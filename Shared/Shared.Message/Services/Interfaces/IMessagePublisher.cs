using Shared.Message.Messages;
using Shared.Message.Options;

namespace Shared.Message.Services.Interfaces;

public interface IMessagePublisher
{
    Task Publish<T>(T message, IMessageOptions options) where T : BaseMessage;
}
