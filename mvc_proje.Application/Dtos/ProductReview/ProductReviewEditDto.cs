using mvc_proje.Domain.Entities;

namespace mvc_proje.Application.Dtos.ProductReview;

public class ProductReviewEditDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int Rating { get; set; }
    public User User { get; set; } = null!;
    public Domain.Entities.Product Product { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    
}