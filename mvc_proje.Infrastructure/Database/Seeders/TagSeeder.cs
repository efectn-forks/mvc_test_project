using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class TagSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "Electronics", Slug = "electronics" },
            new Tag { Id = 2, Name = "Books", Slug = "books" },
            new Tag { Id = 3, Name = "Clothing", Slug = "clothing" },
            new Tag { Id = 4, Name = "Home & Kitchen", Slug = "home-kitchen" },
            new Tag { Id = 5, Name = "Sports & Outdoors", Slug = "sports-outdoors" }
        };

        modelBuilder.Entity<Tag>().HasData(tags);
    }
}