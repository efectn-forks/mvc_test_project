using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class StockTransactionSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var transactions = new List<StockTransaction>
        {
            new StockTransaction
            {
                Id = 1,
                ProductId = 1,
                TransactionType = Domain.Enums.TransactionType.Adjustment,
                Change = 100,
                Description = "Initial stock adjustment for Product 1",
            },
            new StockTransaction
            {
                Id = 2,
                ProductId = 2,
                TransactionType = Domain.Enums.TransactionType.Purchase,
                Change = 100,
                Description = "Purchase stock for Product 2",
            },
            new StockTransaction
            {
                Id = 3,
                ProductId = 3,
                TransactionType = Domain.Enums.TransactionType.Purchase,
                Change = 10,
                Description = "Purchase stock for Product 3",
            },
            new StockTransaction
            {
                Id = 4,
                ProductId = 4,
                TransactionType = Domain.Enums.TransactionType.Purchase,
                Change = 20,
                Description = "Purchase stock for Product 4",
            },
            new StockTransaction
            {
                Id = 5,
                ProductId = 5,
                TransactionType = Domain.Enums.TransactionType.Purchase,
                Change = 30,
                Description = "Purchase stock for Product 5",
            },
        };

        modelBuilder.Entity<StockTransaction>().HasData(transactions);
    }
}