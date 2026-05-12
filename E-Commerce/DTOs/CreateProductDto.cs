using E_Commerce.Models;

namespace E_Commerce.DTOs
{
    public class CreateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus ProductStatus { get; set; }

        public int CategoryId { get; set; }

        public List<IFormFile>? Images { get; set; }

        public List<ExistingImageDto>? ExistingImages { get; set; }

        public List<int>? DeletedImageIds { get; set; }
    }
}
