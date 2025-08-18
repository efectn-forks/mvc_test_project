using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface IPostRepository : ISlugRepository<Post>
{
    Task<IEnumerable<Post>> GetRecentPostsAsync(int count = 5);
}