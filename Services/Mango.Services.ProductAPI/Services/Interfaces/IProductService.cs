using Mango.Services.ProductAPI.Models.Api;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductService
    {
        Task<List<ProductApi>> Get();

        Task<ProductApi> Get(Guid productId);

        Task<ProductApi> AddUpdate(ProductApi product);

        Task<bool> Remove(Guid productId);
    }
}
