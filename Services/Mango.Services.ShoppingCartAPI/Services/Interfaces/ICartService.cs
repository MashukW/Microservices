using Mango.Services.ShoppingCartAPI.Models.Dto;
using Shared.Models.OperationResults;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<Result<CartDto>> Get(Guid userId);

        Task<Result<CartDto>> Create(CartDto cartDto);

        Task<Result<bool>> Clear(Guid cartId);
    }
}
