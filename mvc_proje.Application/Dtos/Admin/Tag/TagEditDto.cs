namespace mvc_proje.Application.Dtos.Admin.Tag;

using mvc_proje.Domain.Entities;

public class TagEditDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
}