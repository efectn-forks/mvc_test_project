using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Post : BaseEntity
{
    [Required]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string? ImageUrl { get; set; } = string.Empty;
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}