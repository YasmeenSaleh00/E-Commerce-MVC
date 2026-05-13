using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _context;

        public CheckoutController(CartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        private string SessionId => _cartService.GetOrCreateSessionId(HttpContext.Session);

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync(UserId, SessionId);
            if (cart == null || !cart.CartItems.Any())
                return RedirectToAction("Index", "Cart");

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(
            string firstName, string lastName,
            string address, string city, string country,
            string? postalCode, string? phone, string? notes)
        {
            var cart = await _cartService.GetCartAsync(UserId, SessionId);
            if (cart == null || !cart.CartItems.Any())
                return RedirectToAction("Index", "Cart");

            decimal subtotal = (decimal)cart.Total;

            var order = new Order
            {
                UserId = UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Paid,
                PaymentMethod = "Credit Card",
                ShippingFirstName = firstName,
                ShippingLastName = lastName,
                ShippingAddress = address,
                ShippingCity = city,
                ShippingCountry = country,
                ShippingPostalCode = postalCode,
                ShippingPhone = phone,
                Notes = notes,
                SubTotal = subtotal,
                ShippingCost = 0,
                Total = subtotal,
                UpdatedAt = DateTime.UtcNow
            };

            foreach (var item in cart.CartItems)
            {
                var img = item.Product?.Images?.FirstOrDefault()?.ImageUrl;
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? string.Empty,
                    ProductImageUrl = img,
                    Quantity = item.Quantity,
                    UnitPrice = (decimal)(item.Product?.Price ?? 0),
                    TotalPrice = (decimal)item.SubTotal
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _cartService.ClearCartAsync(UserId, SessionId);

            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == UserId);

            if (order == null) return NotFound();
            return View(order);
        }
    }
}
