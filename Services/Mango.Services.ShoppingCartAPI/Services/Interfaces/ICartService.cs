using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<CartOutgoing> Get();

        Task<CartOutgoing> AddItem(CartItemIncoming cartItemIncoming);

        Task<bool> RemoveItem(Guid cartItemPublicId);

        Task<bool> ApplyCoupon(string couponCode);

        Task<bool> RemoveCoupon();

        Task<bool> Checkout(CheckoutIncoming incomingCheckout);

        Task<bool> Clear();
    }
}
