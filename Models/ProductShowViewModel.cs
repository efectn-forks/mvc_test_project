using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class ProductShowViewModel
{
    public Product Product { get; set; }
    public IEnumerable<Product> RelatedProducts { get; set; } = new List<Product>();
}