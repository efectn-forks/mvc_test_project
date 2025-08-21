using Microsoft.AspNetCore.Http;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Dtos.Admin.Product;

public class ProductEditDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string SkuNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public IFormFile? Image { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    
    public List<Domain.Entities.ProductFeature> ProductFeatures { get; set; } = new();
    
    public List<int> DeletedImageIds { get; set; } = new List<int>();
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    
    public int? MainExistingImageId { get; set; }
    public int? MainNewFileIndex    { get; set; }
    
    public List<Domain.Entities.ProductReview> ProductReviews { get; set; } = new List<Domain.Entities.ProductReview>();
    public PagedResult<Domain.Entities.StockTransaction> StockTransactions { get; set; } = new PagedResult<Domain.Entities.StockTransaction>();
    public int Stock { get; set; }
}