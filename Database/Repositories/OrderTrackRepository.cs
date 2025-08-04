using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class OrderTrackRepository
{
    private readonly AppDbCtx _context;

    public OrderTrackRepository(AppDbCtx context)
    {
        _context = context;
    }

    public async Task<OrderTrack> GetOrderTrackById(int id)
    {
        return await _context.OrderTrack.FindAsync(id);
    }

    public async Task CreateOrderTrackAsync(OrderTrack orderTrack)
    {
        await _context.OrderTrack.AddAsync(orderTrack);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteOrderTrackAsync(int id)
    {
        var orderTrack = await _context.OrderTrack.FindAsync(id);
        if (orderTrack != null)
        {
            _context.OrderTrack.Remove(orderTrack);
            await _context.SaveChangesAsync();
        }
    }
}