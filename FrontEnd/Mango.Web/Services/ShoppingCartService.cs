using AutoMapper;
using Mango.Web.Accessors.Interfaces;
using Mango.Web.Models.Api.Carts;
using Mango.Web.Models.Api.Checkouts;
using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Checkouts;
using Shared.Models.OperationResults;
using Shared.Models.Requests;
using Shared.Services.Interfaces;

namespace Mango.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ITokenAccessor _tokenAccessor;

        private readonly IMapper _mapper;

        private readonly IHttpService _httpService;
        private readonly ICouponService _couponService;

        public ShoppingCartService(
            ITokenAccessor tokenAccessor,
            IMapper mapper,
            IHttpService httpService,
            ICouponService couponService)
        {
            _tokenAccessor = tokenAccessor;

            _mapper = mapper;

            _httpService = httpService;
            _couponService = couponService;
        }

        public async Task<Result<CartView>> Get()
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(AppConstants.ShoppingCartApi, $"api/cart", HttpMethod.Get, token);

            var getCartResult = await _httpService.Send<CartApi>(requestDetails);
            if (getCartResult != null && getCartResult.IsSuccess && getCartResult.Data != null)
            {
                var cartView = await GetCartView(getCartResult.Data);
                return cartView;
            }

            return new CartView();
        }

        public async Task<Result<CartView>> AddItems(List<CartItemView> cartItems)
        {
            var token = await _tokenAccessor.GetAccessToken();

            var cartItemsApi = _mapper.Map<List<CartItemApi>>(cartItems);
            var requestDetails = RequestData.Create(cartItemsApi, AppConstants.ShoppingCartApi, $"api/cart/add-items", HttpMethod.Post, token);

            var addCartItemsResult = await _httpService.Send<CartApi>(requestDetails);
            if (addCartItemsResult != null && addCartItemsResult.IsSuccess && addCartItemsResult.Data != null)
            {
                var cartView = await GetCartView(addCartItemsResult.Data);
                return cartView;
            }

            return new CartView();
        }

        public async Task<Result<CartView>> UpdateItems(List<CartItemView> cartItems)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var cartItemsApi = _mapper.Map<List<CartItemApi>>(cartItems);
            var requestDetails = RequestData.Create(cartItemsApi, AppConstants.ShoppingCartApi, $"api/cart/update-items", HttpMethod.Put, token);

            var updateCartItemsResult = await _httpService.Send<CartApi>(requestDetails);
            if (updateCartItemsResult != null && updateCartItemsResult.IsSuccess && updateCartItemsResult.Data != null)
            {
                var cartView = await GetCartView(updateCartItemsResult.Data);
                return cartView;
            }

            return new CartView();
        }

        public async Task<Result<bool>> RemoveItems(List<Guid> cartItemPublicIds)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(cartItemPublicIds, AppConstants.ShoppingCartApi, $"api/cart/remove-items", HttpMethod.Delete, token);

            var removeCartItemResponse = await _httpService.Send<bool>(requestDetails);
            return removeCartItemResponse;
        }

        public async Task<Result<bool>> ApplyCoupon(string couponCode)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(couponCode, AppConstants.ShoppingCartApi, $"api/cart/apply-coupon", HttpMethod.Post, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }

        public async Task<Result<bool>> RemoveCoupon()
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(AppConstants.ShoppingCartApi, $"api/cart/remove-coupon", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }

        public async Task<Result<bool>> Clear()
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = RequestData.Create(AppConstants.ShoppingCartApi, $"api/cart/clear", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }

        public async Task<Result<bool>> Checkout(CheckoutView checkout)
        {
            var token = await _tokenAccessor.GetAccessToken();

            var checkoutApi = _mapper.Map<CheckoutApi>(checkout);
            var requestCheckout = RequestData.Create(checkoutApi, AppConstants.ShoppingCartApi, $"api/cart/checkout", HttpMethod.Post, token);

            var checkoutResponse = await _httpService.Send<bool>(requestCheckout);
            return checkoutResponse;
        }

        private async Task<CartView> GetCartView(CartApi cart)
        {
            var cartView = _mapper.Map<CartView>(cart);

            if (!string.IsNullOrWhiteSpace(cartView.CouponCode))
            {
                var couponResponse = await _couponService.Get(cartView.CouponCode);
                if (couponResponse.IsSuccess)
                {
                    cartView.DiscountAmount = couponResponse.Data?.DiscountAmount ?? 0;
                }
            }

            return cartView;
        }
    }
}
