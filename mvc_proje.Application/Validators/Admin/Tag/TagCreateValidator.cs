using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Tag;

namespace mvc_proje.Application.Validators.Admin.Tag;

public class TagCreateValidator : AbstractValidator<TagCreateDto>
{
    public TagCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Etiket adı boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Etiket adı 50 karakterden uzun olamaz.");
        RuleFor(x => x.Description).MaximumLength(200)
            .WithMessage("Etiket açıklaması 200 karakterden uzun olamaz.");
    }
}