using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username, Func<IQueryable<User>, IQueryable<User>>? includeFunc = null);
    Task<User?> GetUsersByUsernameOrEmailAsync(string username, string email);
}