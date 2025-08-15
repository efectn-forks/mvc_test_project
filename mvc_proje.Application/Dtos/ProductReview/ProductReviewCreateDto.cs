namespace mvc_proje.Application.Dtos.ProductReview;

public class ProductReviewCreateDto
{
    public int ProductId { get; set; }
    public string Text { get; set; } = null!;
    public int Rating { get; set; }
}