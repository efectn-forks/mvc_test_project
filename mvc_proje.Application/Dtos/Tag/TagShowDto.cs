using mvc_proje.Application.Dtos.Admin.Tag;

namespace mvc_proje.Application.Dtos.Tag;

using Entities = mvc_proje.Domain.Entities;

public class TagShowDto
{
    public Entities.Tag Tag { get; set; }
    public TagDto Tags { get; set; } = new TagDto();
    public IEnumerable<Entities.Post> LatestPosts { get; set; } = new List<Entities.Post>();
}