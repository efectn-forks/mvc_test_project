using FluentValidation;
using mvc_proje.Application.Dtos.Admin.ProductOption;

namespace mvc_proje.Application.Validators.Admin.ProductOption;

public class ProductOptionCreateValidator : AbstractValidator<ProductOptionCreateDto>
{
    public ProductOptionCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("İsim boş bırakılamaz.")
            .MaximumLength(100).WithMessage("İsim en fazla 100 karakter olmalıdır.");
        
        RuleFor(x => x.Values)
            .NotEmpty().WithMessage("Değerler boş bırakılamaz.")
            .Must(values => values.Split(',').All(v => !string.IsNullOrWhiteSpace(v)))
            .WithMessage("Değerler boş olamaz veya sadece boşluk içermemelidir.");
    }
}