using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class FeatureSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var features = new List<Feature>
        {
            new Feature
            {
                Id = 1, Title = "Waterproof", 
                Description = "Product is waterproof",
                Icon = "fa-solid fa-water",
            },
            new Feature { Id = 2, 
                Title = "Bluetooth", 
                Description = "Product supports Bluetooth connectivity" ,
                Icon = "fa-solid fa-bluetooth"
            },
            new Feature
            {
                Id = 3, 
                Title = "Energy Efficient", 
                Description = "Product is energy efficient",
                Icon = "fa-solid fa-bolt"
            },
            new Feature { 
                Id = 4, 
                Title = "Smart Technology", 
                Description = "Product includes smart technology features",
                Icon = "fa-solid fa-lightbulb"
            },
            new Feature
            {
                Id = 5, 
                Title = "Eco-Friendly", 
                Description = "Product is made from eco-friendly materials",
                Icon = "fa-solid fa-leaf"
            }
        };

        modelBuilder.Entity<Feature>().HasData(features);
    }
}