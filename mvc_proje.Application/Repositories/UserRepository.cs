using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbCtx context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username, Func<IQueryable<User>, IQueryable<User>>? includeFunc = null)
    {
        var query = _context.Users.AsQueryable();
        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUsersByUsernameOrEmailAsync(string username, string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
    }
}