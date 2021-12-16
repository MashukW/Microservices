using AutoMapper;
using Mango.Services.ProductAPI.Database.Entities;
using Mango.Services.ProductAPI.Models.Api;
using Mango.Services.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Models.OperationResults;

namespace Mango.Services.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> productRepository, IWorkUnit workUnit, IMapper mapper)
        {
            _productRepository = productRepository;
            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductApi>>> Get()
        {
            List<Product> products = await _productRepository.Get();
            return _mapper.Map<List<ProductApi>>(products);
        }

        public async Task<Result<ProductApi>> Get(Guid productId)
        {
            var product = await _productRepository.Query(x => x.PublicId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductApi>(product);
        }

        public async Task<Result<ProductApi>> AddUpdate(ProductApi productDto)
        {
            var product = _mapper.Map<ProductApi, Product>(productDto);
            if (product.PublicId == Guid.Empty)
            {
                await _productRepository.Add(product);
            }
            else
            {
                await _productRepository.Update(product);
            }

            await _workUnit.SaveChanges();

            return _mapper.Map<Product, ProductApi>(product);
        }

        public async Task<Result<bool>> Remove(Guid productId)
        {
            try
            {
                var product = await _productRepository.Query(x => x.PublicId == productId).FirstOrDefaultAsync();
                if (product == null)
                {
                    return false;
                }

                await _productRepository.Remove(product);
                await _workUnit.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
