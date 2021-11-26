using Mango.Web.Models.Carts;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface IShoppingCartService
    {
        Task<Result<CartDto>> Get(Guid userId, string? token = null);

        Task<Result<CartDto>> Add(CartDto cartDto, string? token = null);

        Task<Result<CartDto>> Update(CartDto cartDto, string? token = null);

        Task<Result<bool>> RemoveItem<T>(Guid cartId, Guid cartItemId, string? token = null);

        Task<Result<bool>> Clear<T>(Guid userId, string? token = null);
    }
}
