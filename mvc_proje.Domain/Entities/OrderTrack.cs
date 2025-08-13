using System.ComponentModel.DataAnnotations;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Domain.Entities;

public class OrderTrack : BaseEntity
{
    [Required] public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    [Required] public TrackStatus Status { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Tracking information cannot exceed 500 characters.")]
    public string TrackingInfo { get; set; } = string.Empty;
}