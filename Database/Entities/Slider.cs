using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class Slider
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Slider title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;
    
    public string? Button1Url { get; set; } = string.Empty;
    public string? Button1Text { get; set; } = string.Empty;
    
    public string? Button2Url { get; set; } = string.Empty;
    public string? Button2Text { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, ErrorMessage = "Slider image URL cannot exceed 500 characters.")]
    public string ImageUrl { get; set; } = string.Empty;
}