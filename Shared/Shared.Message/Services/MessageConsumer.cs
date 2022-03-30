using Shared.Message.Options;

namespace Shared.Message.Factory
{
    public abstract class MessageConsumer<TOptions, THandler>
        where TOptions : IMessageOptions
    {
        protected THandler MainHandler;
        protected TOptions ConsumeMessageOptions;

        public MessageConsumer(TOptions consumeMessageOptions, THandler mainHandler)
        {
            MainHandler = mainHandler;
            ConsumeMessageOptions = consumeMessageOptions;
        }
    }
}
