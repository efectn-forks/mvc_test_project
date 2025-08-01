using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class TagIndexViewModel
{
    public List<Tag> Tags { get; set; } = new List<Tag>();
}