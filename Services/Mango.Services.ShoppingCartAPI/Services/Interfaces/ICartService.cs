using Mango.Services.ShoppingCartAPI.Models.Dto;
using Shared.Models.OperationResults;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<Result<CartDto>> Get();

        Task<Result<CartDto>> AddItems(List<CartItemDto> cartItemsDto);

        Task<Result<CartDto>> UpdateItems(List<CartItemDto> cartItemsDto);

        Task<Result<bool>> RemoveItems(List<Guid> cartItemsPublicIds);

        Task<Result<bool>> ApplyCoupon(string couponCode);

        Task<Result<bool>> RemoveCoupon();

        Task<Result<bool>> Clear();
    }
}
