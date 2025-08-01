using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    
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
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}