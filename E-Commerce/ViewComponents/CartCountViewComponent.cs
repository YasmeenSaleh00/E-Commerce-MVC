using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly CartService _cartService;

        public CartCountViewComponent(CartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var sessionId = _cartService.GetOrCreateSessionId(HttpContext.Session);
            var count = await _cartService.GetCartCountAsync(
                string.IsNullOrEmpty(userId) ? null : userId,
                sessionId);
            return Content(count.ToString());
        }
    }
}
