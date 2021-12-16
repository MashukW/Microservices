using Mango.Web.Models.View.Checkouts;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public CheckoutController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> CheckoutIndex()
        {
            CheckoutView checkout = new();

            var cartResult = await _shoppingCartService.Get();
            if (cartResult.IsSuccess && cartResult.Data != null)
            {
                checkout.CartItems = cartResult.Data.CartItems;
                checkout.CouponCode = cartResult.Data.CouponCode;
                checkout.TotalCost = cartResult.Data.TotalCost;
                checkout.DiscountAmount = cartResult.Data.DiscountAmount;
            }

            return View(checkout);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutIndex(CheckoutView checkout)
        {
            if (ModelState.IsValid)
            {
                var response = await _shoppingCartService.Checkout(checkout);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Confirmation));
                }
            }

            return View(checkout);
        }

        [HttpGet]
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
