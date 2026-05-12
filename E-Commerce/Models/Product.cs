namespace E_Commerce.Models
{
    public enum ProductStatus
    {
        Available,
        OutOfStock,
        ComingSoon,
        Discontinued
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate  { get; set; }
        public ProductStatus ProductStatus { get; set; }
        
        public List<ProductImage> Images { get; set; }
    }
}
