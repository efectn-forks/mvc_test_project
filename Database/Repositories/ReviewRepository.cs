using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class ReviewRepository
{
    private readonly AppDbCtx _context;

    public ReviewRepository(AppDbCtx context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Reviews.Include(r => r.User).ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews.Include(r => r.User).Where(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var review = await GetByIdAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}