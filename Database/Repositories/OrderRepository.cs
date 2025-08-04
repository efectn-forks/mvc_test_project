using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class OrderRepository
{
    private readonly AppDbCtx _dbContext;

    public OrderRepository(AppDbCtx dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .Include(o => o.OrderTracks)
            .ToListAsync();
    }
    
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .Include(o => o.OrderTracks)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public async Task CreateOrderAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateOrderAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteOrderAsync(int id)
    {
        var order = await GetOrderByIdAsync(id);
        if (order != null)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}