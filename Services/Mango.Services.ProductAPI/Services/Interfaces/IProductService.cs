using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductService
    {
        Task<List<ProductDto>> Get();

        Task<ProductDto> Get(Guid productId);

        Task<ProductDto> AddUpdate(ProductDto product);

        Task<bool> Remove(Guid productId);
    }
}
