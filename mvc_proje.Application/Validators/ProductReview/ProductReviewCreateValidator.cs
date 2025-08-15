using FluentValidation;
using mvc_proje.Application.Dtos.ProductReview;

namespace mvc_proje.Application.Validators.ProductReview;

public class ProductReviewCreateValidator : AbstractValidator<ProductReviewCreateDto>
{
    public ProductReviewCreateValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
    }
}