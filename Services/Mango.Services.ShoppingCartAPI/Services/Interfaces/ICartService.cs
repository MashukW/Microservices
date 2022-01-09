using Mango.Services.ShoppingCartAPI.Models.Api;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<CartApi> Get();

        Task<CartApi> AddItems(List<CartItemApi> cartItemsApi);

        Task<CartApi> UpdateItems(List<CartItemApi> cartItemsApi);

        Task<bool> RemoveItems(List<Guid> cartItemsPublicIds);

        Task<bool> ApplyCoupon(string couponCode);

        Task<bool> RemoveCoupon();

        Task<bool> Checkout(CheckoutApi checkout);

        Task<bool> Clear();
    }
}
