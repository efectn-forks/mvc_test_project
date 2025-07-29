using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Database.Seeders;

public interface ISeeder
{
    public void Seed(ModelBuilder modelBuilder);
}