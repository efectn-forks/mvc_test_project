using mvc_proje.Domain.Enums;

namespace mvc_proje.Application.Dtos.Admin.StockTransaction;

public class StockTransactionCreateDto
{
    public int ProductId { get; set; }
    public int Change { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public TransactionType TransactionType { get; set; }
}