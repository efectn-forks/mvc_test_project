using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public enum TrackStatus
{
    Pending,
    Shipped,
    Delivered,
    Cancelled
}

public class OrderTrack
{
    [Key] public int Id { get; set; }

    [Required] public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    [Required] public TrackStatus Status { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Tracking information cannot exceed 500 characters.")]
    public string TrackingInfo { get; set; } = string.Empty;

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}