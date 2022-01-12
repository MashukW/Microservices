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

            var cartView = await _shoppingCartService.Get();
            if (cartView != null)
            {
                checkout.CartItems = cartView.CartItems;
                checkout.CouponCode = cartView.CouponCode;
                checkout.TotalCost = cartView.TotalCost;
                checkout.DiscountAmount = cartView.DiscountAmount;
            }

            return View(checkout);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutIndex(CheckoutView checkout)
        {
            if (ModelState.IsValid)
            {
                var response = await _shoppingCartService.Checkout(checkout);
                if (response)
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
