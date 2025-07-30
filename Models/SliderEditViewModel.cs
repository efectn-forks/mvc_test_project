using System.ComponentModel.DataAnnotations;
using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class SliderCreateViewModel
{
    [Required]
    [StringLength(100, ErrorMessage = "Slider title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;
    
    public string? Button1Url { get; set; } = string.Empty;
    public string? Button1Text { get; set; } = string.Empty;
    
    public string? Button2Url { get; set; } = string.Empty;
    public string? Button2Text { get; set; } = string.Empty;
    
    public IFormFile? Image { get; set; }
}

public class SliderEditViewModel : SliderCreateViewModel
{
    [Required]
    public int Id { get; set; }
}