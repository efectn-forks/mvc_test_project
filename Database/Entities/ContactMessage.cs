using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class ContactMessage
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, ErrorMessage = "Subject cannot exceed 50 characters.")]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
    public string Message { get; set; } = string.Empty;
}