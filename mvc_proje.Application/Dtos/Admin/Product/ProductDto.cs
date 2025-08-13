namespace mvc_proje.Application.Dtos.Admin.Product;

using mvc_proje.Domain.Entities;

public class ProductDto
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
}