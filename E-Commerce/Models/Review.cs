using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Review:MainEntity
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; } = false; 

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

      
    }
}
