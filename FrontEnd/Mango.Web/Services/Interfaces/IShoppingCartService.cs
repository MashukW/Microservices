using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Checkouts;

namespace Mango.Web.Services
{
    public interface IShoppingCartService
    {
        Task<CartView> Get();

        Task<CartView> AddItem(CartItemView cartItem);

        Task<bool> RemoveItem(Guid cartItemPublicIds);

        Task<bool> ApplyCoupon(string couponCode);

        Task<bool> RemoveCoupon();

        Task<string> Checkout(CheckoutView checkout);

        Task<bool> Clear();
    }
}
