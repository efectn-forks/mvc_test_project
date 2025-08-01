using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Etiket adı gereklidir.")]
    [StringLength(50, ErrorMessage = "Etiket adı en fazla 50 karakter olabilir.")]
    public string Name { get; set; }
    
    [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
    public string? Description { get; set; }
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}