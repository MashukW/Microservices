using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Checkouts;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface IShoppingCartService
    {
        Task<Result<CartView>> Get();

        Task<Result<CartView>> AddItems(List<CartItemView> cartItems);

        Task<Result<CartView>> UpdateItems(List<CartItemView> cartItems);

        Task<Result<bool>> RemoveItems(List<Guid> cartItemPublicIds);

        Task<Result<bool>> ApplyCoupon(string couponCode);

        Task<Result<bool>> RemoveCoupon();

        Task<Result<bool>> Checkout(CheckoutView checkout);

        Task<Result<bool>> Clear();
    }
}
