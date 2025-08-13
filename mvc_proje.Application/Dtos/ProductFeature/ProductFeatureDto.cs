namespace mvc_proje.Application.Dtos.ProductFeature;

using mvc_proje.Domain.Entities;

public class ProductFeatureDto
{
    public List<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();
}