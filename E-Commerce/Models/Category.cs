namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; } 
        public DateTime ModificationDate { get; set; } 

        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public List<Product>? Product { get; set; }
    }
}
