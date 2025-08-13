using mvc_proje.Application.Dtos.Admin.Tag;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Application.Dtos.Post;

using Entities = mvc_proje.Domain.Entities;

public class PostShowDto
{
    public Entities.Post Post { get; set; }
    public TagDto Tags { get; set; } = new TagDto();
    public IEnumerable<Entities.Post> LatestPosts { get; set; } = new List<Entities.Post>();
}