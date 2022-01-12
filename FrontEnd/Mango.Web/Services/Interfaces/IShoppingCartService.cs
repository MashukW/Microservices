using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Checkouts;

namespace Mango.Web.Services
{
    public interface IShoppingCartService
    {
        Task<CartView> Get();

        Task<CartView> AddItems(List<CartItemView> cartItems);

        Task<CartView> UpdateItems(List<CartItemView> cartItems);

        Task<bool> RemoveItems(List<Guid> cartItemPublicIds);

        Task<bool> ApplyCoupon(string couponCode);

        Task<bool> RemoveCoupon();

        Task<bool> Checkout(CheckoutView checkout);

        Task<bool> Clear();
    }
}
