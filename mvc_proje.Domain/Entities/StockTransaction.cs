using mvc_proje.Domain.Enums;

namespace mvc_proje.Domain.Entities;

public class StockTransaction : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Change { get; set; }

    public string Description { get; set; } = string.Empty;

    public TransactionType TransactionType { get; set; }
}