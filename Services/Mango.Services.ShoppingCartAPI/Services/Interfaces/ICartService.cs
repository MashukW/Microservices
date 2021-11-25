using Mango.Services.ShoppingCartAPI.Models.Dto;
using Shared.Models;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<OperationResult<CartDto>> Get(Guid userId);

        Task<OperationResult<CartDto>> Create(CartDto cartDto);

        Task<OperationResult<bool>> Clear(Guid cartId);
    }
}
