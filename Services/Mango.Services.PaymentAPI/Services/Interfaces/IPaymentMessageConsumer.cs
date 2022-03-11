namespace Mango.Services.OrderAPI.Services.Interfaces
{
    public interface IPaymentMessageConsumer
    {
        Task Start();

        Task Stop();
    }
}
