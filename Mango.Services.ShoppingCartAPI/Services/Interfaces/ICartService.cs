using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICartService
    {
        Task<CartDto> Get(Guid userId);

        Task<CartDto> AddUpdate(CartDto cartDto);

        Task<bool> RemoveItem(Guid cartId, Guid cartItemId);

        Task<bool> Clear(Guid cartId);
    }
}
