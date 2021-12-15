using Mango.Web.Models.View.Products;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        Task<Result<List<ProductView>>> Get();

        Task<Result<ProductView>> Get(Guid productId);

        Task<Result<ProductView>> Add(ProductView product);

        Task<Result<ProductView>> Update(ProductView product);

        Task<Result<bool>> Remove(Guid productId);
    }
}
