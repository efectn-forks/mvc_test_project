using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbCtx context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetRelatedProductsAsync(int productId, int count = 5)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return Enumerable.Empty<Product>();
        }

        return await _context.Products
            .Where(p => p.Id != productId)
            .Where(p => p.CategoryId == product.CategoryId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}