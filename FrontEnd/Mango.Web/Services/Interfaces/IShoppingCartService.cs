using Mango.Web.Models;
using Mango.Web.Models.Carts;
using Shared.Models.Responses;

namespace Mango.Web.Services
{
    public interface IShoppingCartService
    {
        Task<ResponseData<CartDto>> Get(Guid userId, string? token = null);

        Task<ResponseData<CartDto>> Add(CartDto cartDto, string? token = null);

        Task<ResponseData<CartDto>> Update(CartDto cartDto, string? token = null);

        Task<ResponseData<bool>> RemoveItem<T>(Guid cartId, Guid cartItemId, string? token = null);

        Task<ResponseData<bool>> Clear<T>(Guid userId, string? token = null);
    }
}
