using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(250, ErrorMessage = "Review text cannot exceed 250 characters.")]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public string UserTitle { get; set; } = string.Empty;
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}