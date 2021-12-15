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
            CartView userCart = new();

            var cartResult = await _shoppingCartService.Get();
            if (cartResult.IsSuccess && cartResult.Data != null)
            {
                userCart = cartResult.Data;
            }

            return View(userCart);
        }

        [HttpPost]
        public async Task<IActionResult> AddItems(Guid PublicId, int count)
        {
            ProductView productView = new();
            var getProductResult = await _productService.Get(PublicId);
            if (getProductResult != null && getProductResult.IsSuccess && getProductResult.Data != null)
            {
                productView = getProductResult.Data;

                var cartItems = new List<CartItemView>
                {
                    new CartItemView
                    {
                        Count = count,
                        Product = _mapper.Map<CartProductView>(productView)
                    }
                };

                var addCartResponse = await _shoppingCartService.AddItems(cartItems);
                if (addCartResponse.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View($"~/Views/Product/ProductDetails.cshtml", productView ?? new ProductView());
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid cartItemPublicId)
        {
            var removeItemsResult = await _shoppingCartService.RemoveItems(new List<Guid> { cartItemPublicId });
            if (removeItemsResult.IsSuccess && removeItemsResult.Data)
            {
                return RedirectToAction(nameof(CartIndex));
            }

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
