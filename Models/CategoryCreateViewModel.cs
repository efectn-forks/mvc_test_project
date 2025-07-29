using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Models;

public class CategoryCreateViewModel
{
    [Required]
    [StringLength(20, ErrorMessage = "Category name cannot exceed 20 characters.")]
    public string Name { get; set; }
    
    [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
    public string Description { get; set; } = string.Empty;
}