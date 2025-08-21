using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class OrderItemSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var orderItems = new List<Domain.Entities.OrderItem>
        {
            new Domain.Entities.OrderItem
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Domain.Entities.OrderItem
            {
                Id = 2,
                OrderId = 1,
                ProductId = 2,
                Quantity = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Domain.Entities.OrderItem
            {
                Id = 3,
                OrderId = 2,
                ProductId = 3,
                Quantity = 3,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Domain.Entities.OrderItem
            {
                Id = 4,
                OrderId = 2,
                ProductId = 4,
                Quantity = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
        };
        
        modelBuilder.Entity<Domain.Entities.OrderItem>().HasData(orderItems);
    }
}