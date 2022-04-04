using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        CartOutgoing Get();

        CartOutgoing AddItem(CartItemIncoming cartItemIncoming);

        bool RemoveItem(Guid cartItemPublicId);

        bool ApplyCoupon(string couponCode);

        bool RemoveCoupon();

        Task<bool> Checkout(CheckoutIncoming incomingCheckout);

        bool Clear();
    }
}
