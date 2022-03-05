namespace Mango.Services.OrderAPI.Services.Interfaces
{
    public interface IOrderMessageConsumer
    {
        Task Start();

        Task Stop();
    }
}
