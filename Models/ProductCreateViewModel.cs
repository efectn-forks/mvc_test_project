using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Models;

public class ProductCreateViewModel
{
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
    
    public IFormFile? Image { get; set; }
}