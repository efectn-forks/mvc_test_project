using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class OrderTrackRepository : GenericRepository<OrderTrack>, IOrderTrackRepository
{
    public OrderTrackRepository(AppDbCtx context) : base(context)
    {
    }
}