using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId,
        Func<IQueryable<Comment>, IQueryable<Comment>>? includeFunc = null);

    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId,
        Func<IQueryable<Comment>, IQueryable<Comment>>? includeFunc = null);

    Task<IEnumerable<Comment>> GetCommentsByParentCommentIdAsync(int parentCommentId);
}