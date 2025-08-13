using System.Linq.Expressions;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
    Task<TEntity?> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);

    // Paged list retrieval
    Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
}