namespace mvc_proje.Application.Dtos.Admin.Tag;

using mvc_proje.Domain.Entities;

public class TagEditDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}