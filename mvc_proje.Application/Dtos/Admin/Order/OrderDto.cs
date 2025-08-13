namespace mvc_proje.Application.Dtos.Admin.Order;

using mvc_proje.Domain.Entities;

public class OrderDto
{
    public IEnumerable<Order> Orders { get; set; } = new List<Order>();
}