namespace mvc_proje.Application.Dtos.Post;

public class LoadMoreProductDto
{
    public string Title { get; set; }
    public string Slug { get; set; }
    
    public decimal Price { get; set; }
    public string MainImageUrl { get; set; }
}