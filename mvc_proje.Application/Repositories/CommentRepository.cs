using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbCtx context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId,
        Func<IQueryable<Comment>, IQueryable<Comment>>? includeFunc = null)
    {
        return await FindAsync(c => c.PostId == postId, includeFunc);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId,
        Func<IQueryable<Comment>, IQueryable<Comment>>? includeFunc = null)
    {
        return await FindAsync(c => c.UserId == userId, includeFunc);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByParentCommentIdAsync(int parentCommentId)
    {
        return await FindAsync(c => c.ParentCommentId == parentCommentId);
    }
}