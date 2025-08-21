using mvc_proje.Application.Dtos.Admin.StockTransaction;
using mvc_proje.Application.Validators.Admin.StockTransaction;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class StockTransactionService
{
    public readonly IUnitOfWork _unitOfWork;
    private readonly StockTransactionCreateValidator _stockTransactionCreateValidator;

    public StockTransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _stockTransactionCreateValidator = new StockTransactionCreateValidator();
    }

    public async Task<PagedResult<Domain.Entities.StockTransaction>> GetStockTransactionsAsync(int pageNumber,
        int pageSize)
    {
        var stockTransactions = await _unitOfWork.StockTransactionRepository.GetPagedAsync(pageNumber, pageSize);
        var totalCount = await _unitOfWork.StockTransactionRepository.CountAsync();

        return new PagedResult<Domain.Entities.StockTransaction>
        {
            Items = stockTransactions,
            TotalCount = totalCount
        };
    }

    public async Task CreateStockTransactionAsync(StockTransactionCreateDto dto)
    {
        var validationResult = await _stockTransactionCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Bazı alanlar geçersiz: " +
                                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }
        
        var stockTransaction = new Domain.Entities.StockTransaction
        {
            ProductId = dto.ProductId,
            Change = dto.Change,
            TransactionType = dto.TransactionType,
            CreatedAt = dto.TransactionDate,
            UpdatedAt = dto.TransactionDate,
        };

        await _unitOfWork.StockTransactionRepository.AddAsync(stockTransaction);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteStockTransactionAsync(int id)
    {
        var stockTransaction = await _unitOfWork.StockTransactionRepository.GetByIdAsync(id);
        if (stockTransaction == null)
        {
            throw new KeyNotFoundException($"{id} ID'li stok işlemi bulunamadı.");
        }

        _unitOfWork.StockTransactionRepository.DeleteAsync(stockTransaction.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}