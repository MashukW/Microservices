using Mango.Web.Models.Products;
using Shared.Models.OperationResults;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>> Get(string token);

        Task<Result<ProductDto>> Get(Guid productId, string token);

        Task<Result<ProductDto>> Add(ProductDto productDto, string token);

        Task<Result<ProductDto>> Update(ProductDto productDto, string token);

        Task<Result<bool>> Remove(Guid productId, string token);
    }
}
