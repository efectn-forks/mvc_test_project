using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class OrderIndexViewModel
{
    public List<Order> Orders { get; set; } = new List<Order>();
}