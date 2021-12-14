using Mango.Web.Models.Carts;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface IShoppingCartService
    {
        Task<Result<CartDto>> Get(string? token = null);

        Task<Result<CartDto>> AddItems(List<CartItemDto> cartItemsDto, string? token);

        Task<Result<CartDto>> UpdateItems(List<CartItemDto> cartItemsDto, string? token);

        Task<Result<bool>> RemoveItems(List<Guid> cartItemPublicIds, string? token);

        Task<Result<bool>> ApplyCoupon(string couponCode, string? token);

        Task<Result<bool>> RemoveCoupon(string? token);

        Task<Result<bool>> Clear(string? token);
    }
}
