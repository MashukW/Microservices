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
            List<ProductView> productViews = new();

            var getProductsResult = await _productService.Get();
            if (getProductsResult != null && getProductsResult.IsSuccess && getProductsResult.Data != null)
            {
                productViews = getProductsResult.Data;
            }

            return View(productViews);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid productId)
        {
            ProductView productView = new();
            var getProductResult = await _productService.Get(productId);
            if (getProductResult != null && getProductResult.IsSuccess && getProductResult.Data != null)
            {
                productView = getProductResult.Data;
            }

            return View(productView);
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
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productView);
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(Guid productId)
        {
            var getProductResult = await _productService.Get(productId);
            if (getProductResult != null && getProductResult.IsSuccess && getProductResult.Data != null)
            {
                var productView = getProductResult.Data;
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
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productView);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(Guid productId)
        {
            var getProductResult = await _productService.Get(productId);
            if (getProductResult != null && getProductResult.IsSuccess && getProductResult.Data != null)
            {
                var productView = getProductResult.Data;
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
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productView);
        }
    }
}