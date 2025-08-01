using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Models;

public class TagCreateViewModel
{
    [Required(ErrorMessage = "Etiket adı gereklidir.")]
    [StringLength(50, ErrorMessage = "Etiket adı en fazla 50 karakter olabilir.")]
    public string Name { get; set; }
    
    [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
    public string? Description { get; set; }
}

public class TagEditViewModel : TagCreateViewModel
{
    [Required]
    public int Id { get; set; }
}