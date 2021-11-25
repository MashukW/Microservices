using Mango.Services.ShoppingCartAPI.Models.Dto;
using Shared.Models;

namespace Mango.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<OperationResult<IList<CartItemDto>>> GetItems(Guid cartId);

        Task<OperationResult<IList<CartItemDto>>> AddUpdateItems(Guid cartId, IList<CartItemDto> cartItemsDto);

        Task<OperationResult<bool>> RemoveItems(Guid cartId, IList<Guid> cartItemIds);
    }
}
