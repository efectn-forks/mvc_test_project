using Microsoft.AspNetCore.Http;

namespace mvc_proje.Application.Dtos.Admin.Product;

public class ProductEditDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SkuNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public IFormFile? Image { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    
    public List<Domain.Entities.ProductFeature> ProductFeatures { get; set; } = new();
}