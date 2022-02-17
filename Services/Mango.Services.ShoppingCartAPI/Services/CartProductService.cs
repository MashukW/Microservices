using AutoMapper;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartProductService : ICartProductService
    {
        private readonly IRepository<CartProduct> _cartProductRepository;

        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartProductService(
            IRepository<CartProduct> cartProductRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _cartProductRepository = cartProductRepository;

            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<int> AddProductIfNotExists(CartProductIncoming cartProductIncoming)
        {
            var cartProduct = await _cartProductRepository
                .Query(x => x.PublicId == cartProductIncoming.PublicId)
                .FirstOrDefaultAsync();

            if (cartProduct != null)
                return cartProduct.Id;

            var newCartProduct = _mapper.Map<CartProduct>(cartProductIncoming);

            await _cartProductRepository.Add(newCartProduct);

            await _workUnit.SaveChanges();

            return newCartProduct.Id;
        }
    }
}
