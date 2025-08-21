namespace mvc_proje.Application.Dtos.Wishlist;

public class WishlistElement
{
    public string Name { get; set; }
    public string SkuNumber { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int ProductId { get; set; }
}

public class WishlistDto
{
    public List<WishlistElement> WishlistItems { get; set; } = new List<WishlistElement>();
}