using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class Feature
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "Feature title cannot exceed 50 characters.")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200, ErrorMessage = "Feature description cannot exceed 200 characters.")]
    public string Description { get; set; } = string.Empty;
    
    public string Icon { get; set; } = string.Empty;
    
    public string Link { get; set; } = string.Empty;
}