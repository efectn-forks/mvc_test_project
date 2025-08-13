using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Infrastructure.Database.Seeders;

public interface ISeeder
{
    public void Seed(ModelBuilder modelBuilder);
}