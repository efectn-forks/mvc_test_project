using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Misc;

public class PagedResult<TEntity> where TEntity : BaseEntity
{
    public IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();
    public int TotalCount { get; set; }
}