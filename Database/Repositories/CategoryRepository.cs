using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class CategoryRepository
{
    private readonly AppDbCtx _dbContext;
    
    public CategoryRepository(AppDbCtx dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _dbContext.Categories.Include(c => c.Products).ToListAsync();
    }
    
    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _dbContext.Categories.Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Category> CreateCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }
    
    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }
    
    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}