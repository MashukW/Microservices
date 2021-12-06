using Mango.Web.Models.Carts;
using Mango.Web.Models.Products;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;

        public CartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> CartIndex()
        {
            CartDto userCart = new();

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shoppingCartService.Get(accessToken);
            if (response.IsSuccess)
            {
                userCart = response.Data ?? new CartDto();
            }

            return View(userCart);
        }

        public async Task<IActionResult> Remove(Guid cartItemPublicId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var removeItemsResponse = await _shoppingCartService.RemoveItems(new List<Guid> { cartItemPublicId }, accessToken);
            if (removeItemsResponse.IsSuccess && removeItemsResponse.Data)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItems(Guid PublicId, int count)
        {
            ProductDto product = new();
            var response = await _productService.Get(PublicId, "");
            if (response.IsSuccess)
            {
                product = response.Data;
            }

            var userPublicId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userPublicId != null && product != null)
            {
                var cartItems = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        Count = count,
                        Product = new CartProductDto
                        {
                            PublicId = product.PublicId,
                            Name = product.Name,
                            Price = product.Price,
                            Description = product.Description,
                            CategoryName = product.CategoryName,
                            ImageUrl = product.ImageUrl,
                        }
                    }
                };

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var addCartResponse = await _shoppingCartService.AddItems(cartItems, accessToken);
                if (addCartResponse.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View($"~/Views/Product/ProductDetails.cshtml", product ?? new ProductDto());
        }
    }
}
