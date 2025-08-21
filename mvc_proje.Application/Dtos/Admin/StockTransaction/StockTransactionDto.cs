namespace mvc_proje.Application.Dtos.Admin.StockTransaction;

public class StockTransactionDto
{
    public IEnumerable<Domain.Entities.StockTransaction> StockTransactions { get; set; }
}