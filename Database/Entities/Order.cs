using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int Price { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}