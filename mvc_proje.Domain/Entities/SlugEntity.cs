using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class SlugEntity : BaseEntity
{
    [Required]
    public string Slug { get; set; } = string.Empty;
}