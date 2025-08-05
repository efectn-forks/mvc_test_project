using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class PostShowViewModel
{
    public Post Post { get; set; }
    public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
    public IEnumerable<Post> LatestPosts { get; set; } = new List<Post>();
}