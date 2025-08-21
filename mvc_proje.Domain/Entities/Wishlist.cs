namespace mvc_proje.Domain.Entities;

public class Wishlist : BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public User User { get; set; }
    public Product Product { get; set; }
}