using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class OrderConfirmationViewModel
{
    public Order Order { get; set; }
    public decimal TotalAmount { get; set; }
    public User User { get; set; }
    public TrackStatus LatestTrackStatus { get; set; }
}