using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        [Required]
        public string Content { get; set; }
        public TestimonialStatus Status { get; set; } = TestimonialStatus.Pending;
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public enum TestimonialStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
