using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Feature;

namespace mvc_proje.Application.Validators.Admin.Feature;

public class FeatureCreateValidator : AbstractValidator<FeatureCreateDto>
{
    public FeatureCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100)
            .WithMessage("Başlık alanı boş bırakılamaz ve 100 karakterden uzun olamaz.");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500)
            .WithMessage("Açıklama alanı boş bırakılamaz ve 500 karakterden uzun olamaz.");
        RuleFor(x => x.Icon).MaximumLength(50)
            .WithMessage("Icon alanı 50 karakterden uzun olamaz.");
        RuleFor(x => x.Link).MaximumLength(200)
            .WithMessage("Bağlantı alanı 200 karakterden uzun olamaz.");
        RuleFor(x => x.Link).Matches(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$")
            .WithMessage("Bağlantı geçerli bir URL formatında olmalıdır.");
    }
}