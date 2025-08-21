using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Category;

namespace mvc_proje.Application.Validators.Admin.Category;

public class CategoryEditValidator : AbstractValidator<CategoryEditDto>
{
    public CategoryEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage("Kategori ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50)
            .WithMessage("Kategori adı boş bırakılamaz ve 50 karakterden uzun olamaz.");
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug alanı boş bırakılamaz.")
            .MaximumLength(100).WithMessage("Slug 100 karakterden uzun olamaz.");
        RuleFor(x => x.Description).MaximumLength(255)
            .NotEmpty().WithMessage("Kategori açıklaması boş bırakılamaz.")
            .WithMessage("Kategori açıklaması 255 karakterden uzun olamaz.");
    }
}