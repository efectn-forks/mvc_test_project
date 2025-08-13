namespace mvc_proje.Application.Dtos.Admin.Review;

public class ReviewEditDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserTitle { get; set; } = string.Empty;
}