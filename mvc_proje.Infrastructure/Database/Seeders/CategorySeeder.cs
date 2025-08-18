using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class CategorySeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics", Slug = "electronics", Description = "Devices and gadgets" },
            new Category
                { Id = 2, Name = "Books", Slug = "books", Description = "Literature and educational materials" },
            new Category { Id = 3, Name = "Clothing", Slug = "clothing", Description = "Apparel and accessories" },
            new Category
            {
                Id = 4, Name = "Home & Kitchen", Slug = "home-kitchen", Description = "Household items and kitchenware"
            },
            new Category
            {
                Id = 5, Name = "Sports & Outdoors", Slug = "sports-outdoors",
                Description = "Equipment for sports and outdoor activities"
            }
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}