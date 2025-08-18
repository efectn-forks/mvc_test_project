using Microsoft.AspNetCore.Http;

namespace mvc_proje.Application.Dtos.Admin.Post;

using mvc_proje.Domain.Entities;

public class PostEditDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    public IFormFile? Image { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Tags { get; set; } = string.Empty;

    public List<string> TagsSplitted => string.IsNullOrEmpty(Tags)
        ? new List<string>()
        : Tags.Split(',').Select(t => t.Trim()).ToList();
}