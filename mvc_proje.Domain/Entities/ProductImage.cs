using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class ProductImage : BaseEntity
{
    [Required]
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters.")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required] public int ProductId { get; set; }

    public Product Product { get; set; } = null!;
}