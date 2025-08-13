using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
    Task<IEnumerable<Comment>> GetCommentsByParentCommentIdAsync(int parentCommentId);
}