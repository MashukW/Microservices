using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Messages;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Database.Repositories;
using Shared.Exceptions;
using Shared.Message;
using Shared.Message.Options.RabbitMq;
using Shared.Message.Services.Interfaces;
using Shared.Models;
using Shared.Options;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly MessageBusOptions _messageBusOptions;
        private readonly IUserAccessor _userAccessor;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ICartProductService _cartProductService;
        private readonly ICouponService _couponService;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IWorkUnit _workUnit;
        private readonly IMapper _mapper;

        public CartService(
            IOptions<MessageBusOptions> messageBusOptions,
            IUserAccessor userAccessor,
            IMessagePublisher messagePublisher,
            ICartProductService cartProductService,
            ICouponService couponService,
            IRepository<Cart> cartRepository,
            IWorkUnit workUnit,
            IMapper mapper)
        {
            _messageBusOptions = messageBusOptions.Value;

            _userAccessor = userAccessor;

            _messagePublisher = messagePublisher;

            _cartProductService = cartProductService;

            _couponService = couponService;

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

            if (!string.IsNullOrWhiteSpace(incomingCheckout.CouponCode))
            {
                var coupon = await _couponService.GetCoupon(incomingCheckout.CouponCode);
                if (incomingCheckout.DiscountAmount != coupon.DiscountAmount)
                {
                    throw new ValidationErrorException(new ValidationMessage
                    {
                        Field = nameof(CheckoutIncoming.DiscountAmount),
                        Messages = new List<string>
                        {
                            "Coupon Price has changed, please confirm"
                        }
                    });
                }
            }

            var excessItems = userCart.CartItems.Select(x => x.PublicId).Except(incomingCheckout.CartItems.Select(x => x.PublicId));
            if (excessItems.Any())
                throw new ValidationErrorException(new ValidationMessage { Field = nameof(incomingCheckout.CartItems), Messages = new List<string> { "Incorrect items in the request" } });

            var checkoutMessage = _mapper.Map<CheckoutMessage>(incomingCheckout);

            // Azure
            // await _messagePublisher.Publish(checkoutMessage, new AzurePublishOptions
            // {
            //     ConnectionString = _messageBusOptions.ConnectionString,
            //     PublishTopicOrQueue = MessageConstants.Azure.Topics.CheckoutOrder
            // });

            // RabbitMq
            await _messagePublisher.Publish(checkoutMessage, new RabbitMqPublishOptions
            {
                HostName = _messageBusOptions.HostName,
                UserName = _messageBusOptions.UserName,
                Password = _messageBusOptions.Password,
                ExchangeName = MessageConstants.RabbitMq.Exchanges.CheckoutDirect,
                RoutingKey = MessageConstants.RabbitMq.RoutingKeys.CheckoutOrder
            });

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

            return userCart ?? new Cart { UserPublicId = appUser.Id };
        }
    }
}
