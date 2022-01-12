using Mango.Web.Models.View.Products;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        Task<List<ProductView>> Get();

        Task<ProductView> Get(Guid productId);

        Task<ProductView> Add(ProductView product);

        Task<ProductView> Update(ProductView product);

        Task<bool> Remove(Guid productId);
    }
}
