using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class MainEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModifcationDate { get; set; }
        public bool? IsDeleted { get; set; } 
    }
}
