using FluentValidation;
using mvc_proje.Application.Dtos.Admin.StockTransaction;

namespace mvc_proje.Application.Validators.Admin.StockTransaction;

public class StockTransactionCreateValidator : AbstractValidator<StockTransactionCreateDto>
{
    public StockTransactionCreateValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty()
            .WithMessage("Ürün ID'si boş bırakılamaz.")
            .GreaterThan(0)
            .WithMessage("Ürün ID'si 0'dan büyük olmalıdır.");
        
        RuleFor(x => x.TransactionType).NotEmpty()
            .WithMessage("İşlem türü boş bırakılamaz.")
            .IsInEnum()
            .WithMessage("Geçersiz işlem türü. Geçerli değerler: StockIn, StockOut.");
    }
}