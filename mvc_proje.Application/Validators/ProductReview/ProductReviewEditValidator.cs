using FluentValidation;
using mvc_proje.Application.Dtos.ProductReview;

namespace mvc_proje.Application.Validators.ProductReview;

public class ProductReviewEditValidator : AbstractValidator<ProductReviewEditDto>
{
    public ProductReviewEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
    }
}