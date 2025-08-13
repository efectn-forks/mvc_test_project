using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbCtx context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await FindAsync(o => o.UserId == userId);
    }
}