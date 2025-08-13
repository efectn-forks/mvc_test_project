using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Comment : BaseEntity
{
    [Required]
    [StringLength(250, ErrorMessage = "Comment text cannot exceed 250 characters.")]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public int? ParentCommentId { get; set; } = null;
    public Comment? ParentComment { get; set; } = null;
}