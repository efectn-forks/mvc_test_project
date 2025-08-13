namespace mvc_proje.Application.Dtos.Admin.Post;

using mvc_proje.Domain.Entities;

public class PostDto
{
    public IEnumerable<Post> Posts { get; set; } = new List<Post>();
}