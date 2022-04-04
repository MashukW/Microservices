using AutoMapper;
using Mango.Services.ShoppingCartAPI.Accessors.Interfaces;
using Mango.Services.ShoppingCartAPI.Models.Entities;
using Mango.Services.ShoppingCartAPI.Models.Incoming;
using Mango.Services.ShoppingCartAPI.Models.Messages;
using Mango.Services.ShoppingCartAPI.Models.Outgoing;
using Mango.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
        private readonly IMemoryCache _cache;
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;

        public CartService(
            IOptions<MessageBusOptions> messageBusOptions,
            IUserAccessor userAccessor,
            IMessagePublisher messagePublisher,
            IMemoryCache cache,
            ICouponService couponService,
            IMapper mapper)
        {
            _messageBusOptions = messageBusOptions.Value;
            _userAccessor = userAccessor;
            _messagePublisher = messagePublisher;
            _cache = cache;
            _couponService = couponService;
            _mapper = mapper;
        }

        public CartOutgoing Get()
        {
            var userCart = GetUserCart();
            return _mapper.Map<CartOutgoing>(userCart);
        }

        public CartOutgoing AddItem(CartItemIncoming cartItemIncoming)
        {
            var userCart = GetUserCart();
            if (userCart.IsInitial())
                userCart.PublicId = Guid.NewGuid();

            var existingCartItem = userCart.CartItems.FirstOrDefault(x => x.Product.PublicId == cartItemIncoming.Product.PublicId);
            if (existingCartItem is null)
            {
                var newCartItem = _mapper.Map<CartItem>(cartItemIncoming);
                userCart.CartItems.Add(newCartItem);
            }
            else
            {
                existingCartItem.Count += cartItemIncoming.Count;
            }

            var userCartKey = string.Format(AppConstants.Cache.UserCartCacheKey, userCart.UserPublicId);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(AppConstants.Cache.UserCartAbsoluteExpirationMin),
            };

            _cache.Set(userCartKey, userCart, cacheEntryOptions);

            return _mapper.Map<CartOutgoing>(userCart);
        }

        public bool RemoveItem(Guid cartItemPublicId)
        {
            var userCart = GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            var cartItem = userCart.CartItems.FirstOrDefault(x => x.PublicId == cartItemPublicId);
            if (cartItem == null)
                return false;

            userCart.CartItems.Remove(cartItem);

            var userCartKey = string.Format(AppConstants.Cache.UserCartCacheKey, userCart.UserPublicId);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(AppConstants.Cache.UserCartAbsoluteExpirationMin),
            };

            _cache.Set(userCartKey, userCart, cacheEntryOptions);

            return true;
        }

        public bool ApplyCoupon(string couponCode)
        {
            var userCart = GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            userCart.CouponCode = couponCode;

            var userCartKey = string.Format(AppConstants.Cache.UserCartCacheKey, userCart.UserPublicId);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(AppConstants.Cache.UserCartAbsoluteExpirationMin),
            };

            _cache.Set(userCartKey, userCart, cacheEntryOptions);

            return true;
        }

        public bool RemoveCoupon()
        {
            var userCart = GetUserCart();
            if (userCart.IsInitial())
                throw new NotFoundException("User cart not found");

            userCart.CouponCode = "";

            var userCartKey = string.Format(AppConstants.Cache.UserCartCacheKey, userCart.UserPublicId);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(AppConstants.Cache.UserCartAbsoluteExpirationMin),
            };

            _cache.Set(userCartKey, userCart, cacheEntryOptions);

            return true;
        }

        public async Task<bool> Checkout(CheckoutIncoming incomingCheckout)
        {
            var userCart = GetUserCart();
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

            Clear();

            return true;
        }

        public bool Clear()
        {
            var userCart = GetUserCart();
            if (userCart.IsInitial())
                return false;

            var userCartKey = string.Format(AppConstants.Cache.UserCartCacheKey, userCart.UserPublicId);
            _cache.Remove(userCartKey);

            return true;
        }

        private Cart GetUserCart()
        {
            var appUser = _userAccessor.GetAppUser();
            if (!appUser.IsValid())
                throw new UnauthorizedException();

            var userCartKey = string.Format(AppConstants.Cache.UserCartCacheKey, appUser.Id);
            if (_cache.TryGetValue(userCartKey, out Cart userCart))
            {
                return userCart;
            }

            return new Cart
            {
                UserPublicId = appUser.Id,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
        }
    }
}
