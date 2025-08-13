namespace mvc_proje.Application.Dtos.Product;

using Entities = mvc_proje.Domain.Entities;

public class ProductShowDto
{
    public Entities.Product Product { get; set; }
    public IEnumerable<Entities.Product> RelatedProducts { get; set; } = new List<Entities.Product>();
}