using System.ComponentModel.DataAnnotations;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Domain.Entities;

public class Order : BaseEntity
{
    [Required]
    public string OrderNumber { get; set; } = string.Empty;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string PaymentToken { get; set; } = string.Empty;
    
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    
    public IEnumerable<OrderItem> OrderItems { get; set; } = null!;
    
    public IEnumerable<OrderTrack> OrderTracks { get; set; } = new List<OrderTrack>();
    
    public OrderTrack GetLatestTrack()
    {
        return OrderTracks.OrderByDescending(track => track.CreatedAt).FirstOrDefault() ?? new OrderTrack();
    }
}