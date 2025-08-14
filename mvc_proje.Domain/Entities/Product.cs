using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace mvc_proje.Domain.Entities;

public class Product : BaseEntity
{
    [Required]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "Sku number cannot exceed 30 characters.")]
    public string SkuNumber { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required] public string Content { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
    public int Stock { get; set; }

    [Required] public int CategoryId { get; set; }

    [JsonIgnore] public Category Category { get; set; } = null!;

    public List<ProductImage> Images { get; set; } = new List<ProductImage>();

    public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public IEnumerable<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();

    public ProductImage GetMainImage()
    {
        return Images.Any(img => img.IsMain)
            ? Images.First(img => img.IsMain)
            : Images.FirstOrDefault() ?? new ProductImage { };
    }
}