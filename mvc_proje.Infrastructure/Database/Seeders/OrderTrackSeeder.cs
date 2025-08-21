using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class OrderTrackSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var orderTracks = new List<Domain.Entities.OrderTrack>
        {
            new Domain.Entities.OrderTrack
            {
                Id = 1,
                OrderId = 1,
                Status = TrackStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Domain.Entities.OrderTrack
            {
                Id = 2,
                OrderId = 1,
                Status = TrackStatus.Shipped,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Domain.Entities.OrderTrack
            {
                Id = 3,
                OrderId = 1,
                Status = TrackStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
        };

        modelBuilder.Entity<Domain.Entities.OrderTrack>().HasData(orderTracks);
    }
}