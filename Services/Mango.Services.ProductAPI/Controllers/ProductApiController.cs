using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Models.Responses;

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
        public async Task<ResponseData<IEnumerable<ProductDto>>> Get()
        {
            try
            {
                var products = await _productService.Get();
                return products;
            }
            catch (Exception ex)
            {
                return ResponseData<IEnumerable<ProductDto>>.Fail(ex.Message);
            }
        }

        [HttpGet("{productId}")]
        public async Task<ResponseData<ProductDto>> Get(Guid productId)
        {
            try
            {
                var product = await _productService.Get(productId);
                return product;
            }
            catch (Exception ex)
            {
                return ResponseData<ProductDto>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ResponseData<ProductDto>> Add([FromBody] ProductDto productDto)
        {
            try
            {
                var product = await _productService.AddUpdate(productDto);
                return product;
            }
            catch (Exception ex)
            {
                return ResponseData<ProductDto>.Fail(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseData<ProductDto>> Update([FromBody] ProductDto productDto)
        {
            try
            {
                var product = await _productService.AddUpdate(productDto);
                return product;
            }
            catch (Exception ex)
            {
                return ResponseData<ProductDto>.Fail(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ResponseData<bool>> Remove(Guid productId)
        {
            try
            {
                var isSuccess = await _productService.Remove(productId);
                return isSuccess;
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Fail(ex.Message);
            }
        }
    }
}
