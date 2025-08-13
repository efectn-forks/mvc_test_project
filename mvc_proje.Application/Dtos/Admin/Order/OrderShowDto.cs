namespace mvc_proje.Application.Dtos.Admin.Order;

using mvc_proje.Domain.Entities;

public class OrderShowDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public IEnumerable<OrderItem> OrderItems { get; set; } = null!;
    public IEnumerable<OrderTrack> OrderTracks { get; set; } = new List<OrderTrack>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public OrderTrack LatestTrack { get; set; } = null!;
}