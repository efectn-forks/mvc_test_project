using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class SliderSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var sliders = new List<Slider>
        {
            new Slider
            {
                Id = 1,
                Title = "Welcome to Our Store",
                ImageUrl = "/images/sliders/carousel-1.jpg",
                Button1Text = "Shop Now",
                Button2Text = "Learn More",
            },
            new Slider
            {
                Id = 2,
                Title = "New Arrivals",
                ImageUrl = "/images/sliders/carousel-2.jpg",
                Button1Text = "View Collection",
                Button2Text = "See Details",
            },
        };

        modelBuilder.Entity<Slider>().HasData(sliders);
    }
}