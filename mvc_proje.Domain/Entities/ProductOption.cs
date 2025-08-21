namespace mvc_proje.Domain.Entities;

public class ProductOption : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public List<ProductOptionValue> Values { get; set; } = new List<ProductOptionValue>();
    public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}