using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class OrderSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                UserId = 1,
                OrderNumber = "TEST-001",
                PaymentStatus = PaymentStatus.Completed,
                PaymentToken = "123abc",
            },
            new Order
            {
                Id = 2,
                UserId = 2,
                OrderNumber = "TEST-002",
                PaymentStatus = PaymentStatus.Pending,
                PaymentToken = "456def",
            },
        };
    }
}