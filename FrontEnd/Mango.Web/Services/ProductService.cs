﻿using AutoMapper;
using Mango.Web.Accessors.Interfaces;
using Mango.Web.Models.Api.Products;
using Mango.Web.Models.View.Products;
using Shared.Models.Requests;
using Shared.Services.Interfaces;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly ITokenAccessor _tokenAccessor;
        private readonly IMapper _mapper;
        private readonly IApiService _httpService;

        public ProductService(IMapper mapper, ITokenAccessor tokenAccessor, IApiService httpService)
        {
            _mapper = mapper;
            _tokenAccessor = tokenAccessor;
            _httpService = httpService;
        }

        public async Task<List<ProductView>> Get()
        {
            var requestDetails = ApiRequest.Create(AppConstants.ProductApiBase, $"api/products/", HttpMethod.Get);

            var getProductsResult = await _httpService.Send<List<ProductApi>>(requestDetails);
            if (getProductsResult != null && getProductsResult.IsSuccess && getProductsResult.Data != null)
            {
                var productsView = _mapper.Map<List<ProductView>>(getProductsResult.Data);
                return productsView;
            }

            return new List<ProductView>();
        }

        public async Task<ProductView> Get(Guid productId)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(AppConstants.ProductApiBase, $"api/products/{productId}", HttpMethod.Get, token);

            var getProductResult = await _httpService.Send<ProductApi>(requestDetails);
            if (getProductResult != null && getProductResult.IsSuccess && getProductResult.Data != null)
            {
                var productView = _mapper.Map<ProductView>(getProductResult.Data);
                return productView;
            }

            return new ProductView();
        }

        public async Task<ProductView> Add(ProductView productDto)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(productDto, AppConstants.ProductApiBase, $"api/products/", HttpMethod.Post, token);

            var addProductResult = await _httpService.Send<ProductApi>(requestDetails);
            if (addProductResult != null && addProductResult.IsSuccess && addProductResult.Data != null)
            {
                var productView = _mapper.Map<ProductView>(addProductResult.Data);
                return productView;
            }

            return new ProductView();
        }

        public async Task<ProductView> Update(ProductView productDto)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(productDto, AppConstants.ProductApiBase, $"api/products/", HttpMethod.Put, token);

            var updateProductResult = await _httpService.Send<ProductApi>(requestDetails);
            if (updateProductResult != null && updateProductResult.IsSuccess && updateProductResult.Data != null)
            {
                var productView = _mapper.Map<ProductView>(updateProductResult.Data);
                return productView;
            }

            return new ProductView();
        }

        public async Task<bool> Remove(Guid productId)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(AppConstants.ProductApiBase, $"api/products/{productId}", HttpMethod.Delete, token);

            var removeProductResponse = await _httpService.Send<bool>(requestDetails);
            return removeProductResponse.Data;
        }
    }
}
