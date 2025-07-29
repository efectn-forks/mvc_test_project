using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Seeders;

public class CategorySeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics", Description = "Devices and gadgets" },
            new Category { Id = 2, Name = "Books", Description = "Literature and educational materials" },
            new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" },
            new Category { Id = 4, Name = "Home & Kitchen", Description = "Household items and kitchenware" },
            new Category { Id = 5, Name = "Sports & Outdoors", Description = "Equipment for sports and outdoor activities" }
        };
        
        modelBuilder.Entity<Category>().HasData(categories);
    }
}