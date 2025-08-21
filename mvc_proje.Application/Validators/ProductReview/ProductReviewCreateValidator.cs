using FluentValidation;
using mvc_proje.Application.Dtos.ProductReview;

namespace mvc_proje.Application.Validators.ProductReview;

public class ProductReviewCreateValidator : AbstractValidator<ProductReviewCreateDto>
{
    public ProductReviewCreateValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage("Ürün ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.Rating).InclusiveBetween(1, 5)
            .WithMessage("Değerlendirme 1 ile 5 arasında olmalıdır.");
        RuleFor(x => x.Text).NotEmpty()
            .WithMessage("Yorum metni boş bırakılamaz.")
            .MaximumLength(500)
            .WithMessage("Yorum metni 500 karakterden uzun olamaz.");
    }
}