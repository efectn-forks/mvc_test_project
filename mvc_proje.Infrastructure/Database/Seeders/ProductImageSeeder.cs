using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class ProductImageSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var productImages = new List<ProductImage>
        {
            new ProductImage
            {
                Id = 1,
                ProductId = 1,
                ImageUrl = "/images/products/product-1.jpg",
                IsMain = true
            },
            new ProductImage
            {
                Id = 2,
                ProductId = 1,
                ImageUrl = "/images/products/product-2.jpg",
                IsMain = false
            },
            new ProductImage
            {
                Id = 3,
                ProductId = 2,
                ImageUrl = "/images/products/product-3.jpg",
                IsMain = true
            },
            new ProductImage
            {
                Id = 4,
                ProductId = 2,
                ImageUrl = "/images/products/product-4.jpg",
                IsMain = false
            },
            new ProductImage
            {
                Id = 5,
                ProductId = 3,
                ImageUrl = "/images/products/product-5.jpg",
                IsMain = true
            },
            new ProductImage
            {
                Id = 6,
                ProductId = 3,
                ImageUrl = "/images/products/product-6.jpg",
                IsMain = false
            },
            new ProductImage
            {
                Id = 7,
                ProductId = 4,
                ImageUrl = "/images/products/product-7.jpg",
                IsMain = false
            },
            new ProductImage
            {
                Id = 8,
                ProductId = 4,
                ImageUrl = "/images/products/product-8.jpg",
                IsMain = true
            },
            new ProductImage
            {
                Id = 9,
                ProductId = 5,
                ImageUrl = "/images/products/product-9.jpg",
                IsMain = true
            },
        };

        modelBuilder.Entity<ProductImage>().HasData(productImages);
    }
}