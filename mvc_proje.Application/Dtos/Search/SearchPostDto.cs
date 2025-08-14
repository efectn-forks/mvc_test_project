namespace mvc_proje.Application.Dtos.Search;

using mvc_proje.Domain.Entities;

public class SearchPostDto
{
    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<Post> RecentPosts { get; set; } = new List<Post>();
    public int TotalCount { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}