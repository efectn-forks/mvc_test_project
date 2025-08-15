namespace mvc_proje.Application.Dtos.ProductReview;

public class ProductReviewEditDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int Rating { get; set; }
}