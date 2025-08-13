using FluentValidation;
using mvc_proje.Application.Dtos.Cart;

namespace mvc_proje.Application.Validators.Cart;

public class CartCreateValidator : AbstractValidator<CartCreateDto>
{
    public CartCreateValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
    }
}