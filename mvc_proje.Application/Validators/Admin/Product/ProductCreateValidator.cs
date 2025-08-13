using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Product;

namespace mvc_proje.Application.Validators.Admin.Product;

public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.SkuNumber).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Stock).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.CategoryId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Image).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png");
    }
}