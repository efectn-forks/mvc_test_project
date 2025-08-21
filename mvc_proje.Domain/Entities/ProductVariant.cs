namespace mvc_proje.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public string SkuNumber { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    public List<ProductOption> ProductOptions { get; set; }
}