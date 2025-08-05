using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Models;

public class CartCreateViewModel
{
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}