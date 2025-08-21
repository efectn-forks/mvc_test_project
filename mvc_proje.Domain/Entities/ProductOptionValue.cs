namespace mvc_proje.Domain.Entities;

public class ProductOptionValue : BaseEntity
{
    public string Value { get; set; } = string.Empty;
    
    public int ProductOptionId { get; set; }
    public ProductOption ProductOption { get; set; }
}