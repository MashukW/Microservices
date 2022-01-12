using Mango.Web.Models.View.Products;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductIndex()
        {
            var productViews = await _productService.Get();

            return View(productViews ?? new List<ProductView>());
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid productId)
        {
            var productView = await _productService.Get(productId);

            return View(productView ?? new ProductView());
        }

        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Add(productView);
                if (response != null)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productView);
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(Guid productId)
        {
            var productView = await _productService.Get(productId);
            if (productView != null)
            {
                return View(productView);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Update(productView);
                if (response != null)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productView);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(Guid productId)
        {
            var productView = await _productService.Get(productId);
            if (productView != null)
            {
                return View(productView);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Remove(productView.PublicId);
                if (response)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productView);
        }
    }
}