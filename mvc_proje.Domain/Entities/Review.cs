using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Review : BaseEntity
{
    [Required]
    [StringLength(250, ErrorMessage = "Review text cannot exceed 250 characters.")]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public string UserTitle { get; set; } = string.Empty;
}