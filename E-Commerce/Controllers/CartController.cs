using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        private string UserId => User.Identity?.IsAuthenticated == true
            ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty
            : string.Empty;

        private string SessionId => _cartService.GetOrCreateSessionId(HttpContext.Session);

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync(
                string.IsNullOrEmpty(UserId) ? null : UserId,
                SessionId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            await _cartService.AddToCartAsync(
                string.IsNullOrEmpty(UserId) ? null : UserId,
                SessionId,
                productId,
                quantity);

            var count = await _cartService.GetCartCountAsync(
                string.IsNullOrEmpty(UserId) ? null : UserId,
                SessionId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, count });

            TempData["SuccessMessage"] = "Item added to cart!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(
                string.IsNullOrEmpty(UserId) ? null : UserId,
                SessionId,
                cartItemId,
                quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(
                string.IsNullOrEmpty(UserId) ? null : UserId,
                SessionId,
                cartItemId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Count()
        {
            var count = await _cartService.GetCartCountAsync(
                string.IsNullOrEmpty(UserId) ? null : UserId,
                SessionId);
            return Json(count);
        }
    }
}
