using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class PostCreateViewModel
{
    [Required]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required] public string Content { get; set; } = string.Empty;

    [Required] public int UserId { get; set; }

    public string? Tags { get; set; } = string.Empty;

    [NotMapped]
    public List<string> TagsSplitted =>
        string.IsNullOrEmpty(Tags) ? new List<string>() : Tags.Split(',').Select(t => t.Trim()).ToList();
}

public class PostEditViewModel : PostCreateViewModel
{
    [Required] public int Id { get; set; }
}