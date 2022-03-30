using Shared.Message.Options;

namespace Shared.Message.Services.Interfaces;

public interface IMessageConsumer
{
    Task StartProcessing(IMessageOptions options, Func<string, Task> processMessage, Func<string, Task>? processError = null);

    Task StopProcessing();
}
