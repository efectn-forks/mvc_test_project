using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.ShoppingCart;

public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public void AddToCart(Product product, int quantity)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Items.Add(new CartItem { ProductId = product.Id, Product = product, Quantity = quantity });
        }
    }

    public void RemoveFromCart(int productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null) Items.Remove(item);
    }

    public void Clear() => Items.Clear();

    public decimal GetTotal() => Items.Sum(i => i.Product.Price * i.Quantity);
}