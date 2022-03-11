namespace Mango.Services.OrderAPI.Services.Interfaces
{
    public interface IUpdatePaymentStatusMessageConsumer
    {
        Task Start();

        Task Stop();
    }
}
