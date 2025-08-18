namespace mvc_proje.Application.Dtos.Admin.Category;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}