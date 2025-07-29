using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Seeders;

namespace mvc_proje.Database;

public class AppDbCtx : DbContext
{
    public AppDbCtx(DbContextOptions<AppDbCtx> options) : base(options)
    {
    }
    
    public AppDbCtx() : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all seeders
        var seeders = new List<ISeeder>
        {
            new CategorySeeder(),
            new ProductSeeder(),
            new UserSeeder(),
            new FeatureSeeder(),
            new ContactMessageSeeder(),
            new PostSeeder()
        };
        
        foreach (var seeder in seeders)
        {
            seeder.Seed(modelBuilder);
        }
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Feature> Features { get; set; } = null!;
    public DbSet<ContactMessage> ContactMessages { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
}