using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Seeders;

using BCrypt.Net;


public class UserSeeder : ISeeder
{
    private string dummyHash = BCrypt.HashPassword("12345678");

    public void Seed(ModelBuilder modelBuilder)
    {
        var users = new List<User>
        {
            new User{
                Id = 1,
                Username = "admin",
                Password = dummyHash,
                Email = "mail1@mail.com",
                FullName = "Admin User",
                PhoneNumber = "1234567890",
                Address = "123 Admin St, Admin City",
                Role = Role.Admin
            },
            new User{
                Id = 2,
                Username = "user1",
                Password = dummyHash,
                Email = "mail2@mail.com",
                FullName = "User One",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User
            },
            new User{
                Id = 3,
                Username = "user3",
                Password = dummyHash,
                Email = "mail3@mail.com",
                FullName = "User Three",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User
            },
            new User{
                Id = 4,
                Username = "user4",
                Password = dummyHash,
                Email = "mail4@mail.com",
                FullName = "User Four",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User
            },
            new User{
                Id = 5,
                Username = "user5",
                Password = dummyHash,
                Email = "mail5@mail.com",
                FullName = "User Five",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User
            },
        };
        
        modelBuilder.Entity<User>().HasData(users);
    }
}