using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        [NotMapped]
        public double Total => CartItems.Sum(i => i.SubTotal);

        [NotMapped]
        public int TotalItems => CartItems.Sum(i => i.Quantity);

    }
}
