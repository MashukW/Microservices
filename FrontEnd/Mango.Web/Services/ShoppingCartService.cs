using Mango.Web.Models.Carts;
using Shared.Models.OperationResults;
using Shared.Models.Requests;
using Shared.Services;

namespace Mango.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IHttpService _httpService;

        public ShoppingCartService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Result<CartDto>> Get(string? token = null)
        {
            var requestDetails = RequestData.Create(AppConstants.ShoppingCartApi, $"api/cart", HttpMethod.Get, token);

            var getCartResponse = await _httpService.Send<CartDto>(requestDetails);

            return getCartResponse;
        }

        public async Task<Result<CartDto>> AddItems(List<CartItemDto> cartItemsDto, string? token)
        {
            var requestDetails = RequestData.Create(cartItemsDto, AppConstants.ShoppingCartApi, $"api/cart/add-items", HttpMethod.Post, token);

            var addCartResponse = await _httpService.Send<CartDto>(requestDetails);
            return addCartResponse;
        }

        public async Task<Result<CartDto>> UpdateItems(List<CartItemDto> cartItemsDto, string? token)
        {
            var requestDetails = RequestData.Create(cartItemsDto, AppConstants.ShoppingCartApi, $"api/cart/update-items", HttpMethod.Put, token);

            var updateCartResponse = await _httpService.Send<CartDto>(requestDetails);
            return updateCartResponse;
        }

        public async Task<Result<bool>> RemoveItems(List<Guid> cartItemPublicIds, string? token = null)
        {
            var requestDetails = RequestData.Create(cartItemPublicIds, AppConstants.ShoppingCartApi, $"api/cart/remove-items", HttpMethod.Delete, token);

            var removeCartItemResponse = await _httpService.Send<bool>(requestDetails);
            return removeCartItemResponse;
        }

        public async Task<Result<bool>> ApplyCoupon(string couponCode, string? token)
        {
            var requestDetails = RequestData.Create(couponCode, AppConstants.ShoppingCartApi, $"api/cart/apply-coupon", HttpMethod.Post, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }

        public async Task<Result<bool>> RemoveCoupon(string? token)
        {
            var requestDetails = RequestData.Create(AppConstants.ShoppingCartApi, $"api/cart/remove-coupon", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }

        public async Task<Result<bool>> Clear(string? token)
        {
            var requestDetails = RequestData.Create(AppConstants.ShoppingCartApi, $"api/cart/clear", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }
    }
}
