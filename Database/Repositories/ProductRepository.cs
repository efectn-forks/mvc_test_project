using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class ProductRepository
{
    private readonly AppDbCtx _dbContext;

    public ProductRepository(AppDbCtx dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.Include(p => p.Category).ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null) return false;

        _dbContext.Products.Remove(product);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<Product>> GetRelatedProductsAsync(int categoryId, int excludeProductId)
    {
        return await _dbContext.Products
            .Where(p => p.CategoryId == categoryId && p.Id == excludeProductId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4).ToListAsync();
    }
}