using Microsoft.AspNetCore.Http;

namespace mvc_proje.Application.Dtos.Admin.Product;

public class ProductCreateDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string SkuNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public IFormFile? Image { get; set; }
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    public int MainImageIndex { get; set; } = 0;
}