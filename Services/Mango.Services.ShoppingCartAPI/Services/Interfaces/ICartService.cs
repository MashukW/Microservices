using Mango.Services.ShoppingCartAPI.Models.Api;
using Shared.Models.OperationResults;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<Result<CartApi>> Get();

        Task<Result<CartApi>> AddItems(List<CartItemApi> cartItemsDto);

        Task<Result<CartApi>> UpdateItems(List<CartItemApi> cartItemsDto);

        Task<Result<bool>> RemoveItems(List<Guid> cartItemsPublicIds);

        Task<Result<bool>> ApplyCoupon(string couponCode);

        Task<Result<bool>> RemoveCoupon();

        Task<Result<bool>> Clear();
    }
}
