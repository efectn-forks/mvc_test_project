using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Tag : SlugEntity
{
    [Required(ErrorMessage = "Etiket adı gereklidir.")]
    [StringLength(50, ErrorMessage = "Etiket adı en fazla 50 karakter olabilir.")]
    public string Name { get; set; }
    
    
    [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
    public string? Description { get; set; }
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}