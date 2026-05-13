using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private const string SessionKey = "CartSessionId";

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetOrCreateSessionId(ISession session)
        {
            var sessionId = session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                session.SetString(SessionKey, sessionId);
            }
            return sessionId;
        }

        public async Task<ShoppingCart?> GetCartAsync(string? userId, string sessionId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return await _context.ShoppingCarts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                            .ThenInclude(p => p.Images)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }

            return await _context.ShoppingCarts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.UserId == null);
        }

        public async Task<ShoppingCart> GetOrCreateCartAsync(string? userId, string sessionId)
        {
            var cart = await GetCartAsync(userId, sessionId);
            if (cart != null) return cart;

            cart = new ShoppingCart
            {
                UserId = userId,
                SessionId = sessionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.ShoppingCarts.Add(cart);
            await _context.SaveChangesAsync();
            cart.CartItems = new List<CartItem>();
            return cart;
        }

        public async Task AddToCartAsync(string? userId, string sessionId, int productId, int quantity = 1)
        {
            var cart = await GetOrCreateCartAsync(userId, sessionId);
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var item = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                };
                _context.CartItems.Add(item);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(string? userId, string sessionId, int cartItemId, int quantity)
        {
            var cart = await GetCartAsync(userId, sessionId);
            if (cart == null) return;

            var item = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (item == null) return;

            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string? userId, string sessionId, int cartItemId)
        {
            var cart = await GetCartAsync(userId, sessionId);
            if (cart == null) return;

            var item = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (item == null) return;

            _context.CartItems.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string? userId, string sessionId)
        {
            var cart = await GetCartAsync(userId, sessionId);
            if (cart == null) return;

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartCountAsync(string? userId, string sessionId)
        {
            var cart = await GetCartAsync(userId, sessionId);
            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }

        // Called after login: move session cart items into the user's cart
        public async Task MergeGuestCartAsync(string userId, string sessionId)
        {
            var guestCart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.UserId == null);

            if (guestCart == null || !guestCart.CartItems.Any()) return;

            var userCart = await GetOrCreateCartAsync(userId, sessionId);

            foreach (var guestItem in guestCart.CartItems)
            {
                var existingItem = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == guestItem.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += guestItem.Quantity;
                }
                else
                {
                    guestItem.CartId = userCart.Id;
                }
            }

            _context.ShoppingCarts.Remove(guestCart);
            await _context.SaveChangesAsync();
        }
    }
}
