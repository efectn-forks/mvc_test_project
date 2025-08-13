using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Slider : BaseEntity
{
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