using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class ProductReviewSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var productReviews = new List<Domain.Entities.ProductReview>
        {
            new Domain.Entities.ProductReview
            {
                Id = 1,
                ProductId = 1,
                UserId = 1,
                Rating = 5,
                Text = "Excellent product!",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Domain.Entities.ProductReview
            {
                Id = 2,
                ProductId = 1,
                UserId = 2,
                Rating = 4,
                Text = "Very good quality.",
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                UpdatedAt = DateTime.UtcNow.AddDays(-8)
            },
            new Domain.Entities.ProductReview
            {
                Id = 3,
                ProductId = 1,
                UserId = 3,
                Rating = 3,
                Text = "Average experience.",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Domain.Entities.ProductReview
            {
                Id = 4,
                ProductId = 1,
                UserId = 4,
                Rating = 2,
                Text = "Not satisfied with the quality.",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
        };

        modelBuilder.Entity<Domain.Entities.ProductReview>().HasData(productReviews);
    }
}