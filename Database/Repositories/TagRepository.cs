using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class TagRepository
{
    private readonly AppDbCtx _context;

    public TagRepository(AppDbCtx context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.Include(t => t.Posts).ToListAsync();
    }

    public async Task<Tag?> GetTagByIdAsync(int id)
    {
        return await _context.Tags
            .Include(t => t.Posts)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddTagAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagAsync(Tag tag)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(int id)
    {
        var tag = await GetTagByIdAsync(id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}