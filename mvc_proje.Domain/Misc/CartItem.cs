using mvc_proje.Domain.Entities;

namespace mvc_proje.Domain.Misc;

public class CartItem
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}