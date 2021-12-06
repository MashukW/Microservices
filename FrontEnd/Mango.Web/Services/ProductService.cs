using Mango.Web.Models.Products;
using Shared.Models.OperationResults;
using Shared.Models.Requests;
using Shared.Services;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpService _httpService;

        public ProductService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Result<List<ProductDto>>> Get(string token)
        {
            var requestDetails = RequestData.Create(AppConstants.ProductApiBase, $"api/products/", HttpMethod.Get, token);

            var getProductsResponse = await _httpService.Send<List<ProductDto>>(requestDetails);
            return getProductsResponse;
        }

        public async Task<Result<ProductDto>> Get(Guid productId, string token)
        {
            var requestDetails = RequestData.Create(AppConstants.ProductApiBase, $"api/products/{productId}", HttpMethod.Get, token);

            var getProductResponse = await _httpService.Send<ProductDto>(requestDetails);
            return getProductResponse;
        }

        public async Task<Result<ProductDto>> Add(ProductDto productDto, string token)
        {
            var requestDetails = RequestData.Create(productDto, AppConstants.ProductApiBase, $"api/products/", HttpMethod.Post, token);

            var addProductResponse = await _httpService.Send<ProductDto>(requestDetails);
            return addProductResponse;
        }

        public async Task<Result<ProductDto>> Update(ProductDto productDto, string token)
        {
            var requestDetails = RequestData.Create(productDto, AppConstants.ProductApiBase, $"api/products/", HttpMethod.Put, token);

            var updateProductResponse = await _httpService.Send<ProductDto>(requestDetails);
            return updateProductResponse;
        }

        public async Task<Result<bool>> Remove(Guid productId, string token)
        {
            var requestDetails = RequestData.Create(AppConstants.ProductApiBase, $"api/products/{productId}", HttpMethod.Delete, token);

            var removeProductResponse = await _httpService.Send<bool>(requestDetails);
            return removeProductResponse;
        }
    }
}
