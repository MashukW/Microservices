using AutoMapper;
using Mango.Web.Accessors.Interfaces;
using Mango.Web.Models.Api.Carts;
using Mango.Web.Models.Api.Checkouts;
using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Checkouts;
using Shared.Models.Requests;
using Shared.Services.Interfaces;

namespace Mango.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ITokenAccessor _tokenAccessor;

        private readonly IMapper _mapper;

        private readonly IApiService _httpService;
        private readonly ICouponService _couponService;

        public ShoppingCartService(
            ITokenAccessor tokenAccessor,
            IMapper mapper,
            IApiService httpService,
            ICouponService couponService)
        {
            _tokenAccessor = tokenAccessor;

            _mapper = mapper;

            _httpService = httpService;
            _couponService = couponService;
        }

        public async Task<CartView> Get()
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(AppConstants.ShoppingCartApi, $"api/cart", HttpMethod.Get, token);

            var getCartResult = await _httpService.Send<CartApi>(requestDetails);
            if (getCartResult != null && getCartResult.IsSuccess && getCartResult.Data != null)
            {
                var cartView = await GetCartView(getCartResult.Data);
                return cartView;
            }

            return new CartView();
        }

        public async Task<CartView> AddItem(CartItemView cartItem)
        {
            var token = await _tokenAccessor.GetAccessToken();

            var cartItemsApi = _mapper.Map<CartItemApi>(cartItem);
            var requestDetails = ApiRequest.Create(cartItemsApi, AppConstants.ShoppingCartApi, $"api/cart/add-item", HttpMethod.Post, token);

            var addCartItemsResult = await _httpService.Send<CartApi>(requestDetails);
            if (addCartItemsResult != null && addCartItemsResult.IsSuccess && addCartItemsResult.Data != null)
            {
                var cartView = await GetCartView(addCartItemsResult.Data);
                return cartView;
            }

            return new CartView();
        }

        public async Task<bool> RemoveItem(Guid cartItemPublicId)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(cartItemPublicId, AppConstants.ShoppingCartApi, $"api/cart/remove-item", HttpMethod.Delete, token);

            var removeCartItemResponse = await _httpService.Send<bool>(requestDetails);
            if (removeCartItemResponse.IsSuccess)
                return removeCartItemResponse.Data;

            return false;
        }

        public async Task<bool> ApplyCoupon(string couponCode)
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(couponCode, AppConstants.ShoppingCartApi, $"api/cart/apply-coupon", HttpMethod.Post, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            if (clearCartResponse.IsSuccess)
                return clearCartResponse.Data;

            return false;
        }

        public async Task<bool> RemoveCoupon()
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(AppConstants.ShoppingCartApi, $"api/cart/remove-coupon", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            if (clearCartResponse.IsSuccess)
                return clearCartResponse.Data;

            return false;
        }

        public async Task<bool> Clear()
        {
            var token = await _tokenAccessor.GetAccessToken();
            var requestDetails = ApiRequest.Create(AppConstants.ShoppingCartApi, $"api/cart/clear", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            if (clearCartResponse.IsSuccess)
                return clearCartResponse.Data;

            return false;
        }

        public async Task<string> Checkout(CheckoutView checkout)
        {
            var token = await _tokenAccessor.GetAccessToken();

            var checkoutApi = _mapper.Map<CheckoutApi>(checkout);
            var requestCheckout = ApiRequest.Create(checkoutApi, AppConstants.ShoppingCartApi, $"api/cart/checkout", HttpMethod.Post, token);

            var checkoutResponse = await _httpService.Send<bool>(requestCheckout);
            if (checkoutResponse.IsSuccess)
                return string.Empty;

            if (checkoutResponse?.ValidationMessages is not null && checkoutResponse.ValidationMessages.Any())
                return string.Join("\n", checkoutResponse.ValidationMessages.SelectMany(x => x?.Messages == null ? new List<string>() : x.Messages));

            if (checkoutResponse?.ErrorMessage is not null)
                return checkoutResponse.ErrorMessage;

            return string.Empty;
        }

        private async Task<CartView> GetCartView(CartApi cart)
        {
            var cartView = _mapper.Map<CartView>(cart);

            if (!string.IsNullOrWhiteSpace(cartView.CouponCode))
            {
                var coupon = await _couponService.Get(cartView.CouponCode);
                cartView.DiscountAmount = coupon?.DiscountAmount ?? 0;
            }

            return cartView;
        }
    }
}
