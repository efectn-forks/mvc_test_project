namespace mvc_proje.Application.Dtos.Admin.Tag;

using mvc_proje.Domain.Entities;

public class TagDto
{
    public IEnumerable<Tag> Tags { get; set; }
}