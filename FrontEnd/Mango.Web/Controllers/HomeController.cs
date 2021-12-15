using Mango.Web.Models;
using Mango.Web.Models.View.Products;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductView> productViews = new();
            var getProductsResult = await _productService.Get();
            if (getProductsResult != null && getProductsResult.IsSuccess && getProductsResult.Data != null)
            {
                productViews = getProductsResult.Data;
            }

            return View(productViews);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}