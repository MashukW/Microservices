using Mango.Services.OrderAPI.Database.Entities;

namespace Mango.Services.OrderAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderOutgoing> Get(Guid publicOrderId);

        Task ChangePaymentStatus(Guid publicOrderId, bool status);
    }
}
