using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class UserRepository
{
    public readonly AppDbCtx _dbCtx;

    public UserRepository(AppDbCtx dbCtx)
    {
        _dbCtx = dbCtx;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbCtx.Users
            .Include(u => u.Orders)
            .Include(u => u.Posts)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _dbCtx.Users
            .Include(u => u.Orders)
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameOrEmailAsync(string username, string email)
    {
        return await _dbCtx.Users
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _dbCtx.Users
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems)
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderTracks)
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        if (user == null) return false;

        await _dbCtx.Users.AddAsync(user);
        return await _dbCtx.SaveChangesAsync() > 0;
    }

    public bool UpdateUser(User user)
    {
        if (user == null) return false;
        _dbCtx.Users.Update(user);
        return _dbCtx.SaveChanges() > 0;
    }

    public bool DeleteUser(int id)
    {
        var user = _dbCtx.Users.Find(id);
        if (user == null) return false;
        
        // remove avatar file if it exists
        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var avatarPath = Path.Combine("wwwroot", "images", "avatars", user.AvatarUrl);
            if (File.Exists(avatarPath))
            {
                File.Delete(avatarPath);
            }
        }

        _dbCtx.Users.Remove(user);
        return _dbCtx.SaveChanges() > 0;
    }
}