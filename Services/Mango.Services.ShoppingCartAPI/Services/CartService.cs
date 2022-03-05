using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Messages;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Database.Repositories;
using Shared.Exceptions;
using Shared.Message.Services.Interfaces;
using Shared.Models;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IMessagePublisher _messageBus;
        private readonly ICartProductService _cartProductService;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartService(
            IUserAccessor userAccessor,
            IMessagePublisher messageBus,
            ICartProductService cartProductService,
            IRepository<Cart> cartRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _userAccessor = userAccessor;

            _messageBus = messageBus;

            _cartProductService = cartProductService;

            _cartRepository = cartRepository;

            _workUnit = workUnit;
            _mapper = mapper;
        }

        public async Task<CartOutgoing> Get()
        {
            var userCart = await GetUserCart();
            return _mapper.Map<CartOutgoing>(userCart);
        }

        public async Task<CartOutgoing> AddItem(CartItemIncoming cartItemIncoming)
        {
            var userCart = await GetUserCart();

            var productId = await _cartProductService.AddProductIfNotExists(cartItemIncoming.Product);

            var cartItem = _mapper.Map<CartItem>(cartItemIncoming);
            cartItem.ProductId = productId;

            if (!userCart.CartItems.Any())
            {
                userCart.CartItems.Add(cartItem);

                if (userCart.IsInitial())
                {
                    await _cartRepository.Add(userCart);
                }
                else
                {
                    await _cartRepository.Update(userCart);
                }
            }
            else
            {
                var existingCartItem = userCart.CartItems.FirstOrDefault(x => x.ProductId == productId);
                if (existingCartItem != null)
                {
                    existingCartItem.Count += cartItemIncoming.Count;
                }
                else
                {
                    userCart.CartItems.Add(cartItem);
                }

                await _cartRepository.Update(userCart);
            }

            await _workUnit.SaveChanges();

            return _mapper.Map<CartOutgoing>(userCart);
        }

        public async Task<bool> RemoveItem(Guid cartItemPublicId)
        {
            var userCart = await GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            var cartItem = userCart.CartItems.FirstOrDefault(x => x.PublicId == cartItemPublicId);
            if (cartItem == null)
                return false;

            userCart.CartItems.Remove(cartItem);
            await _cartRepository.Update(userCart);

            await _workUnit.SaveChanges();

            return true;
        }

        public async Task<bool> ApplyCoupon(string couponCode)
        {
            var userCart = await GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            userCart.CouponCode = couponCode;
            await _cartRepository.Update(userCart);
            await _workUnit.SaveChanges();

            return true;
        }

        public async Task<bool> RemoveCoupon()
        {
            var userCart = await GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            userCart.CouponCode = "";
            await _cartRepository.Update(userCart);
            await _workUnit.SaveChanges();

            return true;
        }

        public async Task<bool> Checkout(CheckoutIncoming incomingCheckout)
        {
            var userCart = await GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            var excessItems = userCart.CartItems.Select(x => x.PublicId).Except(incomingCheckout.CartItems.Select(x => x.PublicId));
            if (excessItems.Any())
                throw new ValidationErrorException(new ValidationMessage { Field = nameof(incomingCheckout.CartItems), Messages = new List<string> { "Incorrect items in the request" } });

            var checkoutMessage = _mapper.Map<CheckoutMessage>(incomingCheckout);
            await _messageBus.Publish(checkoutMessage, "checkout");

            await Clear();

            return true;
        }

        public async Task<bool> Clear()
        {
            var userCart = await GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            userCart.CartItems?.Clear();
            userCart.CouponCode = string.Empty;

            await _cartRepository.Update(userCart);

            await _workUnit.SaveChanges();

            return true;
        }

        private async Task<Cart> GetUserCart()
        {
            var appUser = _userAccessor.GetAppUser();
            if (!appUser.IsValid())
                throw new UnauthorizedException();

            var userCart = await _cartRepository
                .Query()
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserPublicId == appUser.Id);

            return userCart ?? new Cart();
        }
    }
}
