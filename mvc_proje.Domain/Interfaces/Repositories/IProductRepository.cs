using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetRelatedProductsAsync(int productId, int count = 5);
}