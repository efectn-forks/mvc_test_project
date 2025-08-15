namespace mvc_proje.Domain.Entities;

public class ProductReview : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Text { get; set; } = null!;
    public int Rating { get; set; }
}