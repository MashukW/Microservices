using Mango.Services.ProductAPI.Models.Api;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.Constants;
using Shared.Models.OperationResults;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ApiResult<List<ProductApi>>> Get()
        {
            try
            {
                var products = await _productService.Get();
                return products;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpGet("{productId}")]
        public async Task<ApiResult<ProductApi>> Get(Guid productId)
        {
            try
            {
                var product = await _productService.Get(productId);
                return product;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ApiResult<ProductApi>> Add([FromBody] ProductApi productDto)
        {
            try
            {
                var product = await _productService.AddUpdate(productDto);
                return product;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ApiResult<ProductApi>> Update([FromBody] ProductApi productDto)
        {
            try
            {
                var product = await _productService.AddUpdate(productDto);
                return product;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ApiResult<bool>> Remove(Guid productId)
        {
            try
            {
                var isSuccess = await _productService.Remove(productId);
                return isSuccess;
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }
    }
}
