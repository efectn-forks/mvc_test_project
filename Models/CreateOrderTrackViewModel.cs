using System.ComponentModel.DataAnnotations;
using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class CreateOrderTrackViewModel
{
    [Required]
    public TrackStatus Status { get; set; } = TrackStatus.Pending;
    
    [Required]
    [StringLength(500, ErrorMessage = "Tracking information cannot exceed 500 characters.")]
    public string TrackingInfo { get; set; } = string.Empty;
    
    [Required]
    public int OrderId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}