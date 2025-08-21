using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class StockTransactionRepository : GenericRepository<StockTransaction>, IStockTransactionRepository
{
    public StockTransactionRepository(AppDbCtx context) : base(context)
    {
    }
}