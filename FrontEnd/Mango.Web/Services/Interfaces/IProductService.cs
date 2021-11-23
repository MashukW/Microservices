using Mango.Web.Models;
using Mango.Web.Models.Products;
using Shared.Models.Responses;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        Task<ResponseData<List<ProductDto>>> Get(string token);

        Task<ResponseData<ProductDto>> Get(Guid productId, string token);

        Task<ResponseData<ProductDto>> Add(ProductDto productDto, string token);

        Task<ResponseData<ProductDto>> Update(ProductDto productDto, string token);

        Task<ResponseData<bool>> Remove(Guid productId, string token);
    }
}
