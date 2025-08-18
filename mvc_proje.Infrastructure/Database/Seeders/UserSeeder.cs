using Microsoft.EntityFrameworkCore;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Infrastructure.Database.Seeders;

public class UserSeeder : ISeeder
{
    private readonly string dummyHash = BCrypt.Net.BCrypt.HashPassword("12345678");

    public void Seed(ModelBuilder modelBuilder)
    {
        var users = new List<User>
        {
            new()
            {
                Id = 1,
                Username = "admin",
                Password = dummyHash,
                Email = "mail1@mail.com",
                FullName = "Admin User",
                PhoneNumber = "1234567890",
                Address = "123 Admin St, Admin City",
                City = "Istanbul",
                ZipCode = "12345",
                Country = "Türkiye",
                IdentifyNumber = "11111111111",
                BirthDate = new DateTime(1990, 1, 1),
                Role = Role.Admin
            },
            new()
            {
                Id = 2,
                Username = "user1",
                Password = dummyHash,
                Email = "mail2@mail.com",
                FullName = "User One",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User,
                City = "Istanbul",
                ZipCode = "12345",
                Country = "Türkiye",
                IdentifyNumber = "11111111111",
                BirthDate = new DateTime(1990, 1, 1),
            },
            new()
            {
                Id = 3,
                Username = "user3",
                Password = dummyHash,
                Email = "mail3@mail.com",
                FullName = "User Three",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User,
                City = "Istanbul",
                ZipCode = "12345",
                Country = "Türkiye",
                IdentifyNumber = "11111111111",
                BirthDate = new DateTime(1990, 1, 1),
            },
            new()
            {
                Id = 4,
                Username = "user4",
                Password = dummyHash,
                Email = "mail4@mail.com",
                FullName = "User Four",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User,
                City = "Istanbul",
                ZipCode = "12345",
                Country = "Türkiye",
                IdentifyNumber = "11111111111",
                BirthDate = new DateTime(1990, 1, 1),
            },
            new()
            {
                Id = 5,
                Username = "user5",
                Password = dummyHash,
                Email = "mail5@mail.com",
                FullName = "User Five",
                PhoneNumber = "0987654321",
                Address = "456 User Ave, User City",
                Role = Role.User,
                City = "Istanbul",
                ZipCode = "12345",
                Country = "Türkiye",
                IdentifyNumber = "11111111111",
                BirthDate = new DateTime(1990, 1, 1),
            }
        };

        modelBuilder.Entity<User>().HasData(users);
    }
}