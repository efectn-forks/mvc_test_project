using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class FeatureSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var features = new List<Feature>
        {
            new Feature { Id = 1, Title = "Waterproof", Description = "Product is waterproof" },
            new Feature { Id = 2, Title = "Bluetooth", Description = "Product supports Bluetooth connectivity" },
            new Feature { Id = 3, Title = "Energy Efficient", Description = "Product is energy efficient" },
            new Feature { Id = 4, Title = "Smart Technology", Description = "Product includes smart technology features" },
            new Feature { Id = 5, Title = "Eco-Friendly", Description = "Product is made from eco-friendly materials" }
        };

        modelBuilder.Entity<Feature>().HasData(features);
    }
}