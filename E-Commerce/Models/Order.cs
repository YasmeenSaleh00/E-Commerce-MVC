using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }

    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [StringLength(100)]
        public string? PaymentMethod { get; set; }

        [StringLength(200)]
        public string ShippingFirstName { get; set; } = string.Empty;

        [StringLength(200)]
        public string ShippingLastName { get; set; } = string.Empty;

        [StringLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(100)]
        public string ShippingCity { get; set; } = string.Empty;

        [StringLength(100)]
        public string ShippingCountry { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ShippingPostalCode { get; set; }

        [StringLength(20)]
        public string? ShippingPhone { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();




    }
}
