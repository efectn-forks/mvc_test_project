using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class PostSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var posts = new List<Post>
        {
            new Post
            {
                Id = 1,
                Title = "Understanding Dependency Injection in ASP.NET Core",
                Content = "Dependency Injection (DI) is a design pattern that allows for the decoupling of classes and their dependencies. In ASP.NET Core, DI is built-in and can be configured in the Startup class.",
                UserId = 1,
            },
            new Post
            {
                Id = 2,
                Title = "Exploring Entity Framework Core",
                Content = "Entity Framework Core (EF Core) is an open-source, lightweight, extensible, and cross-platform version of Entity Framework. It is used to access databases in .NET applications.",
                UserId = 1,
            }
        };

        modelBuilder.Entity<Post>().HasData(posts);
    }
}