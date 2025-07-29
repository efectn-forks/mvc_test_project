using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Seeders;

public class ContactMessageSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var contactMessages = new List<ContactMessage>
        {
            new ContactMessage
            {
                Id = 1,
                Name = "John Doe",
                Email = "test@test.com",
                Subject = "Test Subject",
                Message = "This is a test message.",
            },
            new ContactMessage
            {
                Id = 2,
                Name = "Jane Smith",
                Email = "test2@test.com",
                Subject = "Another Test Subject",
                Message = "This is another test message.",
            },
        };
        
        modelBuilder.Entity<ContactMessage>().HasData(contactMessages);
    }
}