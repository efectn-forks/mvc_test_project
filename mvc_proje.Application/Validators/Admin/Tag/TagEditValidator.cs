using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Tag;

namespace mvc_proje.Application.Validators.Admin.Tag;

public class TagEditValidator : AbstractValidator<TagEditDto>
{
    public TagEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage("Etiket ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Etiket adı boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Etiket adı 50 karakterden uzun olamaz.");
        RuleFor(x => x.Description).MaximumLength(200)
            .WithMessage("Etiket açıklaması 200 karakterden uzun olamaz.");
    }
}