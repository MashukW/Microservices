using Mango.Web.Models;
using Mango.Web.Models.Carts;
using Mango.Web.Models.Products;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public HomeController(IProductService productService, IShoppingCartService shoppingCartService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();
            var response = await _productService.Get("");
            if (response.IsSuccess)
            {
                products = response.Data;
            }

            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid productId)
        {
            ProductDto product= new();
            var response = await _productService.Get(productId, "");
            if (response.IsSuccess)
            {
                product = response.Data;
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(Guid productId, int count)
        {
            ProductDto product = new();
            var response = await _productService.Get(productId, "");
            if (response.IsSuccess)
            {
                product = response.Data;
            }

            var userPublicId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userPublicId != null && product != null)
            {
                var cartDto = new CartDto
                {
                    UserPublicId = Guid.Parse(userPublicId),
                    CouponCode = "",
                    CartItems = new List<CartItemDto>
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
                    }
                };

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var addCartResponse = await _shoppingCartService.Add(cartDto, accessToken);
                if (addCartResponse.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}