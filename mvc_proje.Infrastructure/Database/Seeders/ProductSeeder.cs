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
                Slug = "product-1",
                Description = "Description for Product 1",
                Content = "This is a detailed description of Product 1. It includes features, specifications, and other relevant information that helps customers understand the product better.",
                Price = 19.99m,
                Stock = 100,
                SkuNumber = "SKU12345",
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 2,
                Name = "Product 2",
                Slug = "product-2",
                Description = "Description for Product 2",
                Content = "This is a detailed description of Product 2. It includes features, specifications, and other relevant information that helps customers understand the product better.",
                Price = 29.99m,
                Stock = 50,
                SkuNumber = "SKU12346",
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 3,
                Name = "Product 3",
                Slug = "product-3",
                Description = "Description for Product 3",
                Content = "This is a detailed description of Product 3. It includes features, specifications, and other relevant information that helps customers understand the product better.",
                Price = 39.99m,
                Stock = 75,
                SkuNumber = "SKU12347",
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 4,
                Name = "Product 4",
                Slug = "product-4",
                Description = "Description for Product 4",
                Content = "This is a detailed description of Product 4. It includes features, specifications, and other relevant information that helps customers understand the product better.",
                Price = 49.99m,
                Stock = 20,
                SkuNumber = "SKU12348",
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
            new Product
            {
                Id = 5,
                Name = "Product 5",
                Slug = "product-5",
                Description = "Description for Product 5",
                Content = "This is a detailed description of Product 5. It includes features, specifications, and other relevant information that helps customers understand the product better.",
                Price = 59.99m,
                Stock = 10,
                SkuNumber = "SKU12349",
                CategoryId = category_ids[new Random().Next(category_ids.Length)],
            },
        };

        modelBuilder.Entity<Product>().HasData(products);
    }
}