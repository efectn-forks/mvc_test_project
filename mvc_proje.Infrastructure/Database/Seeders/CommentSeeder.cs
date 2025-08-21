using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class CommentSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var comments = new List<Domain.Entities.Comment>
        {
            new Domain.Entities.Comment
            {
                Id = 1,
                PostId = 1,
                UserId = 1,
                Text = "Great product!",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Domain.Entities.Comment
            {
                Id = 2,
                PostId = 1,
                UserId = 2,
                ParentCommentId = 1,
                Text = "I love this!",
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                UpdatedAt = DateTime.UtcNow.AddDays(-8)
            },
            new Domain.Entities.Comment
            {
                Id = 3,
                PostId = 2,
                UserId = 3,
                Text = "Very informative post.",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Domain.Entities.Comment
            {
                Id = 4,
                PostId = 2,
                UserId = 4,
                ParentCommentId = 3,
                Text = "I agree, very helpful!",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
        };

        modelBuilder.Entity<Domain.Entities.Comment>().HasData(comments);
    }
}