using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Incoming;

namespace Mango.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICartProductService
    {
        Task<int> AddProductIfNotExists(CartProductIncoming cartProductIncoming);
    }
}
