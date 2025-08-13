using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Application.Dtos.Order;

public class OrderConfirmationDto
{
    public Domain.Entities.Order Order { get; set; }
    public decimal TotalAmount { get; set; }
    public User User { get; set; }
    public TrackStatus LatestTrackStatus { get; set; }
}