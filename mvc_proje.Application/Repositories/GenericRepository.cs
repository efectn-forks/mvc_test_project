using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbCtx _context;

    public GenericRepository(AppDbCtx context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        IQueryable<TEntity> res = _context.Set<TEntity>()
            // .AsNoTracking()
            .AsQueryable();

        if (includeFunc != null)
        {
            res = includeFunc(res);
        }

        return await res.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        IQueryable<TEntity> res = _context.Set<TEntity>()
            // .AsNoTracking()
            .AsQueryable();

        if (includeFunc != null)
        {
            res = includeFunc(res);
        }

        return await res.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>()
            // .AsNoTracking()
            .Where(predicate);
        
        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}