using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Testimonial:MainEntity
    {
        [Required]
        public string Content { get; set; }
        public bool IsApproved { get; set; } = false;
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
