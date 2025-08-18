using System.Linq.Expressions;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface ISlugRepository<TEntity> : IGenericRepository<TEntity> where TEntity : SlugEntity
{
    Task<TEntity?> GetBySlugAsync(string slug, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task<bool> SlugExistsAsync(string slug, Expression<Func<TEntity, bool>>? predicate = null);
}