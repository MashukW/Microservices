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

        public async Task<Result<CartDto>> Get(Guid userId, string? token = null)
        {
            var requestDetails = RequestData.Get(AppConstants.ShoppingCartApi, $"api/cart/{userId}", HttpMethod.Get, token);

            var getCartResponse = await _httpService.Send<CartDto>(requestDetails);

            return getCartResponse;
        }

        public async Task<Result<CartDto>> Add(CartDto cartDto, string? token = null)
        {
            var requestDetails = RequestData.Get(cartDto, AppConstants.ShoppingCartApi, $"api/cart/add", HttpMethod.Post, token);

            var addCartResponse = await _httpService.Send<CartDto>(requestDetails);
            return addCartResponse;
        }

        public async Task<Result<CartDto>> Update(CartDto cartDto, string? token = null)
        {
            var requestDetails = RequestData.Get(cartDto, AppConstants.ShoppingCartApi, $"api/cart/update/", HttpMethod.Put, token);

            var updateCartResponse = await _httpService.Send<CartDto>(requestDetails);
            return updateCartResponse;
        }

        public async Task<Result<bool>> RemoveItem<T>(Guid cartId, Guid cartItemId, string? token = null)
        {
            var requestDetails = RequestData.Get(AppConstants.ShoppingCartApi, $"api/cart/remove-item/{cartId}/{cartItemId}", HttpMethod.Delete, token);

            var removeCartItemResponse = await _httpService.Send<bool>(requestDetails);
            return removeCartItemResponse;
        }

        public async Task<Result<bool>> Clear<T>(Guid userId, string? token = null)
        {
            var requestDetails = RequestData.Get(AppConstants.ShoppingCartApi, $"api/cart/clear/{userId}", HttpMethod.Delete, token);

            var clearCartResponse = await _httpService.Send<bool>(requestDetails);
            return clearCartResponse;
        }
    }
}
