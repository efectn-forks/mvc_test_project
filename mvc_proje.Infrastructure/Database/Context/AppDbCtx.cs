using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using mvc_proje.Domain.Entities;
using mvc_proje.Infrastructure.Database.Seeders;

namespace mvc_proje.Infrastructure.Database.Context;

public class AppDbCtx : DbContext
{
    public AppDbCtx(DbContextOptions<AppDbCtx> options) : base(options)
    {
    }
    
    public AppDbCtx()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
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
            new PostSeeder(),
            new StockTransactionSeeder(),
            new ProductImageSeeder(),
            new SliderSeeder(),
            new OrderSeeder(),
            //new OrderTrackSeeder(),
            new ProductReviewSeeder(),
            new ReviewSeeder(),
            new CommentSeeder(),
            new TagSeeder(),
        };
        
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
        
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Slug)
            .IsUnique();
        
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Slug)
            .IsUnique();
        
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();
        
        modelBuilder.Entity<Post>()
            .HasIndex(c => c.Slug)
            .IsUnique();
        
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);
        
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
    public DbSet<Slider> Sliders { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<OrderTrack> OrderTrack { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<ProductReview> ProductReviews { get; set; } = null!;
    public DbSet<StockTransaction> StockTransactions { get; set; } = null!;
    public DbSet<ProductOption> ProductOptions { get; set; } = null!;
    public DbSet<ProductOptionValue> ProductOptionValues { get; set; } = null!;
    public  DbSet<ProductVariant> ProductVariants { get; set; } = null!;
}