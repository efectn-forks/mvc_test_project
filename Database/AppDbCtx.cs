using Microsoft.EntityFrameworkCore;

namespace mvc_proje.Database;

public class AppDbCtx : DbContext
{
    public AppDbCtx(DbContextOptions<AppDbCtx> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
    
    
}