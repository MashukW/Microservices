namespace Mango.Services.EmailAPI.Services.Interfaces
{
    public interface IEmailMessageConsumer
    {
        Task Start();

        Task Stop();
    }
}
