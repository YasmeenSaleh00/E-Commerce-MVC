using Microsoft.AspNetCore.Identity;
using Microsoft.Identity;
using System.ComponentModel.DataAnnotations;
namespace E_Commerce.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public string Gender { get; set; }

    }
}
