using AutoMapper;
using Mango.Web.Models.View.Carts;
using Mango.Web.Models.View.Products;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;

        public CartController(
            IMapper mapper,
            IShoppingCartService shoppingCartService,
            ICouponService couponService,
            IProductService productService)
        {
            _mapper = mapper;

            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> CartIndex()
        {
            var userCart = await _shoppingCartService.Get();
            return View(userCart);
        }

        [HttpPost]
        public async Task<IActionResult> AddItems(Guid PublicId, int count)
        {
            var productView = await _productService.Get(PublicId);
            if (productView != null)
            {
                var cartItemView = new CartItemView
                {
                    Count = count,
                    Product = _mapper.Map<CartProductView>(productView)
                };

                await _shoppingCartService.AddItem(cartItemView);
                return RedirectToAction("Index", "Home");
            }

            return View($"~/Views/Product/ProductDetails.cshtml", productView ?? new ProductView());
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid cartItemPublicId)
        {
            var removeItemsResult = await _shoppingCartService.RemoveItem(cartItemPublicId);
            if (removeItemsResult)
                return RedirectToAction(nameof(CartIndex));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartView cartView)
        {
            await _shoppingCartService.ApplyCoupon(cartView.CouponCode);
            return RedirectToAction("CartIndex");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon()
        {
            await _shoppingCartService.RemoveCoupon();
            return RedirectToAction("CartIndex");
        }
    }
}
