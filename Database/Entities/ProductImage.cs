using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class ProductImage
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters.")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required] public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}