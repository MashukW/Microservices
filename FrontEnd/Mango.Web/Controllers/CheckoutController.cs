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
                checkout.Cart = cartResult.Data;
            }

            return View(checkout);
        }
    }
}
