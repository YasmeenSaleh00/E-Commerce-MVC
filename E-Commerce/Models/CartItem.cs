using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public ShoppingCart Cart { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public double SubTotal => (Product?.Price ?? 0) * Quantity;


    }
}
