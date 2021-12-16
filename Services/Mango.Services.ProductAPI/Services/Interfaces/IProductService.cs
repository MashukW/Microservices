using Mango.Services.ProductAPI.Models.Api;
using Shared.Models.OperationResults;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductService
    {
        Task<Result<List<ProductApi>>> Get();

        Task<Result<ProductApi>> Get(Guid productId);

        Task<Result<ProductApi>> AddUpdate(ProductApi product);

        Task<Result<bool>> Remove(Guid productId);
    }
}
