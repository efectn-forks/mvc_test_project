using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Domain.Entities;

public class Order : BaseEntity
{
    [Required]
    public string OrderNumber { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public IEnumerable<OrderItem> OrderItems { get; set; } = null!;
    
    public IEnumerable<OrderTrack> OrderTracks { get; set; } = new List<OrderTrack>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public OrderTrack GetLatestTrack()
    {
        return OrderTracks.OrderByDescending(track => track.CreatedAt).FirstOrDefault() ?? new OrderTrack();
    }
}