using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Feature : BaseEntity
{
    [Required]
    [StringLength(50, ErrorMessage = "Feature title cannot exceed 50 characters.")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200, ErrorMessage = "Feature description cannot exceed 200 characters.")]
    public string Description { get; set; } = string.Empty;
    
    public string? Icon { get; set; } = string.Empty;
    
    public string? Link { get; set; } = string.Empty;
}