using Mango.Web.Models.Products;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
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
            List<ProductDto> products = new();

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.Get(accessToken);
            if (response.IsSuccess)
            {
                products = response.Data;
            }

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid productId)
        {
            ProductDto product = new();
            var response = await _productService.Get(productId, "");
            if (response.IsSuccess)
            {
                product = response.Data;
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.Add(productDto, accessToken);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(Guid productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");


            var response = await _productService.Get(productId, accessToken);
            if (response.IsSuccess)
            {
                var product = response.Data;
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.Update(productDto, accessToken);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(Guid productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.Get(productId, accessToken);
            if (response.IsSuccess)
            {
                var product = response.Data;
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.Remove(productDto.PublicId, accessToken);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(productDto);
        }
    }
}