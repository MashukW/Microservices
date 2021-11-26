using Mango.Services.ShoppingCartAPI.Models.Dto;
using Shared.Models.OperationResults;

namespace Mango.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<Result<IList<CartItemDto>>> GetItems(Guid cartId);

        Task<Result<IList<CartItemDto>>> AddUpdateItems(Guid cartId, IList<CartItemDto> cartItemsDto);

        Task<Result<bool>> RemoveItems(Guid cartId, IList<Guid> cartItemIds);
    }
}
