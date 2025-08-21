using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class ReviewSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var reviews = new List<Domain.Entities.Review>
        {
            new Domain.Entities.Review
            {
                Id = 1,
                UserId = 1,
                UserTitle = "John Doe",
                Text = "Great product!",
            },
            new Domain.Entities.Review
            {
                Id = 2,
                UserId = 2,
                UserTitle = "Jane Smith",
                Text = "Very informative post.",
            },
            new Domain.Entities.Review
            {
                Id = 3,
                UserId = 3,
                UserTitle = "Alice Johnson",
                Text = "I love this!",
            },
            new Domain.Entities.Review
            {
                Id = 4,
                UserId = 4,
                UserTitle = "Bob Brown",
                Text = "Not satisfied with the quality.",
            },
        };

        modelBuilder.Entity<Domain.Entities.Review>().HasData(reviews);
    }
}