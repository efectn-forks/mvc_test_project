using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class CommentRepository
{
    public readonly AppDbCtx _dbCtx;

    public CommentRepository(AppDbCtx dbCtx)
    {
        _dbCtx = dbCtx;
    }

    public async Task<List<Comment>> GetComments()
    {
        return await _dbCtx.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
            .Include(c => c.ParentComment)
            .ToListAsync();
    }
    
    public async Task<Comment?> GetCommentById(int id)
    {
        return await _dbCtx.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
            .Include(c => c.ParentComment)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task AddComment(Comment comment)
    {
        _dbCtx.Comments.Add(comment);
        await _dbCtx.SaveChangesAsync();
    }
    
    public async Task UpdateComment(Comment comment)
    {
        _dbCtx.Comments.Update(comment);
        await _dbCtx.SaveChangesAsync();
    }
    
    public async Task DeleteComment(int id)
    {
        var comment = await GetCommentById(id);
        if (comment != null)
        {
            _dbCtx.Comments.Remove(comment);
            await _dbCtx.SaveChangesAsync();
        }
    }
    
    public async Task<List<Comment>> GetCommentsByPostId(int postId)
    {
        return await _dbCtx.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
            .Include(c => c.ParentComment)
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }
}