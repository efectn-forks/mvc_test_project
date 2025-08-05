using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class TagShowViewModel
{
    public Tag Tag { get; set; }
    public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
    public IEnumerable<Post> LatestPosts { get; set; } = new List<Post>();
}