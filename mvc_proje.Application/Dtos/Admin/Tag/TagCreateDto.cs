namespace mvc_proje.Application.Dtos.Admin.Tag;

using mvc_proje.Domain.Entities;

public class TagCreateDto
{
    public string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
}