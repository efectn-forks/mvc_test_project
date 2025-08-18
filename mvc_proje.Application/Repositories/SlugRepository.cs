using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class SlugRepository<TEntity> : GenericRepository<TEntity> where TEntity : SlugEntity
{
    public SlugRepository(AppDbCtx context) : base(context)
    {
    }
    
    public async Task<TEntity?> GetBySlugAsync(string slug, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        
        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.FirstOrDefaultAsync(e => e.Slug == slug);
    }
    
    public async Task<bool> SlugExistsAsync(string slug, Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.AnyAsync(e => e.Slug == slug);
    }
}