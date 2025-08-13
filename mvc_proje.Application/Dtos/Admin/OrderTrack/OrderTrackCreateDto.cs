using System.ComponentModel.DataAnnotations;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Application.Dtos.Admin.OrderTrack;

public class OrderTrackCreateDto
{
    public TrackStatus Status { get; set; } = TrackStatus.Pending;
    public string TrackingInfo { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}