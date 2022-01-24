using Mango.Services.ShoppingCartAPI.Models.Api;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<CartOutgoing> Get();

        Task<CartOutgoing> AddItems(List<CartItemIncoming> cartItemsIncoming);

        Task<CartOutgoing> UpdateItems(List<CartItemIncoming> cartItemsIncoming);

        Task<bool> RemoveItems(List<Guid> cartItemsPublicIds);

        Task<bool> ApplyCoupon(string couponCode);

        Task<bool> RemoveCoupon();

        Task<bool> Checkout(CheckoutIncoming checkoutIncoming);

        Task<bool> Clear();
    }
}
