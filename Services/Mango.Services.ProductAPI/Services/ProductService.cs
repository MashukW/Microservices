using AutoMapper;
using Mango.Services.ProductAPI.Database.Entities;
using Mango.Services.ProductAPI.Models.Api;
using Mango.Services.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Exceptions;

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

        public async Task<List<ProductApi>> Get()
        {
            List<Product> products = await _productRepository.Get();
            return _mapper.Map<List<ProductApi>>(products);
        }

        public async Task<ProductApi> Get(Guid productId)
        {
            var product = await _productRepository.Query(x => x.PublicId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductApi>(product);
        }

        public async Task<ProductApi> AddUpdate(ProductApi productDto)
        {
            Product? product = null;
            if (productDto.PublicId == Guid.Empty)
            {
                product = _mapper.Map<ProductApi, Product>(productDto);
                await _productRepository.Add(product);
            }
            else
            {
                product = await _productRepository.Query().FirstOrDefaultAsync(x => x.PublicId == productDto.PublicId);
                if (product != null)
                {
                    product.Name = productDto.Name;
                    product.CategoryName = productDto.CategoryName;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                    product.ImageUrl = productDto.ImageUrl;

                    await _productRepository.Update(product);
                }
            }

            await _workUnit.SaveChanges();

            return _mapper.Map<Product, ProductApi>(product ?? new Product());
        }

        public async Task<bool> Remove(Guid productId)
        {
            var product = await _productRepository.Query(x => x.PublicId == productId).FirstOrDefaultAsync();
            if (product == null)
                throw new NotFoundException($"Product '{product}' not found");

            await _productRepository.Remove(product);
            await _workUnit.SaveChanges();

            return true;
        }
    }
}
