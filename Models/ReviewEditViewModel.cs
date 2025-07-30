using System.ComponentModel.DataAnnotations;
using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class ReviewCreateViewModel
{
    [Required]
    [StringLength(250, ErrorMessage = "Review text cannot exceed 250 characters.")]
    public string Text { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string UserTitle { get; set; } = string.Empty;
}

public class ReviewEditViewModel : ReviewCreateViewModel
{
    [Required]
    public int Id { get; set; }
}