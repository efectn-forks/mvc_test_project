using FluentValidation;
using mvc_proje.Application.Dtos.ProductFeature;

namespace mvc_proje.Application.Validators.Admin.ProductFeature;

public class ProductFeatureCreateValidator : AbstractValidator<ProductFeatureCreateDto>
{
    public ProductFeatureCreateValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Ürün ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Anahtar alanı boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Anahtar 100 karakterden uzun olamaz.");
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Değer alanı boş bırakılamaz.")
            .MaximumLength(255)
            .WithMessage("Değer 255 karakterden uzun olamaz.");
    }
}