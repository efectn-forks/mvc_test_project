using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class ProductSeeder : ISeeder
{
    private int[] category_ids = { 1, 2, 3, 4, 5 };
    public void Seed(ModelBuilder modelBuilder)
    {
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "Description for Product 1",
                Price = 19.99m,
                Stock = 100,
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 2,
                Name = "Product 2",
                Description = "Description for Product 2",
                Price = 29.99m,
                Stock = 50,
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 3,
                Name = "Product 3",
                Description = "Description for Product 3",
                Price = 39.99m,
                Stock = 75,
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 4,
                Name = "Product 4",
                Description = "Description for Product 4",
                Price = 49.99m,
                Stock = 20,
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 5,
                Name = "Product 5",
                Description = "Description for Product 5",
                Price = 59.99m,
                Stock = 10,
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
        };

        modelBuilder.Entity<Product>().HasData(products);
    }
}