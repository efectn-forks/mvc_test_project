using FluentValidation;
using mvc_proje.Application.Dtos.Cart;

namespace mvc_proje.Application.Validators.Cart;

public class CartCreateValidator : AbstractValidator<CartCreateDto>
{
    public CartCreateValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Ürün ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.Quantity).NotEmpty()
            .WithMessage("Miktar boş bırakılamaz.")
            .GreaterThan(0)
            .WithMessage("Miktar 0'dan büyük olmalıdır.");
    }
}