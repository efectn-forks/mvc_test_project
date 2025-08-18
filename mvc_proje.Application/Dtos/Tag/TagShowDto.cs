using mvc_proje.Application.Dtos.Admin.Tag;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Dtos.Tag;

using Entities = mvc_proje.Domain.Entities;

public class TagShowDto
{
    public Entities.Tag Tag { get; set; }
    public PagedResult<Entities.Post> PagedPosts { get; set; } = new PagedResult<Entities.Post>();
    public TagDto Tags { get; set; } = new TagDto();
    public IEnumerable<Entities.Post> LatestPosts { get; set; } = new List<Entities.Post>();
}