﻿using Mango.Services.ProductAPI.Models.Api;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Models.ApiResponses;

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
        public async Task<ApiResponse<List<ProductApi>>> Get()
        {
            var products = await _productService.Get();
            return products;
        }

        [HttpGet("{productId}")]
        public async Task<ApiResponse<ProductApi>> Get(Guid productId)
        {
            var product = await _productService.Get(productId);
            return product;
        }

        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<ProductApi>> Add([FromBody] ProductApi productDto)
        {
            var product = await _productService.AddUpdate(productDto);
            return product;
        }

        [HttpPut]
        [Authorize]
        public async Task<ApiResponse<ProductApi>> Update([FromBody] ProductApi productDto)
        {
            var product = await _productService.AddUpdate(productDto);
            return product;
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ApiResponse<bool>> Remove(Guid productId)
        {
            var isSuccess = await _productService.Remove(productId);
            return isSuccess;
        }
    }
}
