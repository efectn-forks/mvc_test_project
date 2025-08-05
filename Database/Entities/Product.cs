using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace mvc_proje.Database.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "Sku number cannot exceed 30 characters.")]
    public string SkuNumber { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
    public int Stock { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [JsonIgnore]
    public Category Category { get; set; } = null!;

    public string? ImageUrl { get; set; } = string.Empty;
    
    public IEnumerable<ProductImage> Images { get; set; } = new List<ProductImage>();
  
    public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}