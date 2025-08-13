namespace mvc_proje.Domain.Entities;

public class ProductFeature : BaseEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}