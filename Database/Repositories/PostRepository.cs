using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class PostRepository
{
    private readonly AppDbCtx _dbContext;
    public PostRepository(AppDbCtx dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _dbContext.Posts.Include(p => p.User).ToListAsync();
    }
    
    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _dbContext.Posts.Include(p => p.User)
            .Include(p => p.Tags)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<Post> CreatePostAsync(Post post)
    {
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
        return post;
    }
    
    public async Task<bool> UpdatePostAsync(Post post)
    {
        post.UpdatedAt = DateTime.Now;
        _dbContext.Posts.Update(post);
        
        var result = await _dbContext.SaveChangesAsync();
        
        return result > 0;
    }
    
    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _dbContext.Posts.FindAsync(id);
        if (post == null) return false;
        
        // remove the image
        if (!string.IsNullOrEmpty(post.ImageUrl))
        {
            var imagePath = Path.Combine("wwwroot", post.ImageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        _dbContext.Posts.Remove(post);
        var result = await _dbContext.SaveChangesAsync();
        
        return result > 0;
    }
    
    public async Task<List<Post>> GetLatestPostsAsync(int count = 5)
    {
        return await _dbContext.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}