using FluentValidation;
using mvc_proje.Application.Dtos.ProductFeature;

namespace mvc_proje.Application.Validators.Admin.ProductFeature;

public class ProductFeatureCreateValidator : AbstractValidator<ProductFeatureCreateDto>
{
    public ProductFeatureCreateValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Value)
            .NotEmpty()
            .MaximumLength(255);
    }
}