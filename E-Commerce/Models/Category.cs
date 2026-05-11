namespace E_Commerce.Models
{
    public class Category:MainEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
    }
}
