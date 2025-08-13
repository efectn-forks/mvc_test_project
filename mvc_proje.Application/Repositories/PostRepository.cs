using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbCtx context) : base(context)
    {
    }

    public async Task<IEnumerable<Post>> GetRecentPostsAsync(int count = 5)
    {
        return await _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}