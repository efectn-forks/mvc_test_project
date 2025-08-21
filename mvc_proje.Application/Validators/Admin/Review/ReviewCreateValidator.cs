using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Review;

namespace mvc_proje.Application.Validators.Admin.Review;

public class ReviewCreateValidator : AbstractValidator<ReviewCreateDto>
{
    public ReviewCreateValidator()
    {
        RuleFor(x => x.Text).NotEmpty()
            .WithMessage("Yorum metni boş bırakılamaz.")
            .MaximumLength(500)
            .WithMessage("Yorum metni 500 karakterden uzun olamaz.");
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage("Kullanıcı ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.UserTitle).NotEmpty()
            .WithMessage("Kullanıcı başlığı boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Kullanıcı başlığı 100 karakterden uzun olamaz.");
    }
}