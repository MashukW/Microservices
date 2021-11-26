using Mango.Services.ProductAPI.Models.Dto;
using Shared.Models.OperationResults;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>> Get();

        Task<Result<ProductDto>> Get(Guid productId);

        Task<Result<ProductDto>> AddUpdate(ProductDto product);

        Task<Result<bool>> Remove(Guid productId);
    }
}
